using System.Xml.Serialization;
using BinaryFormatDataStructure;

namespace ModernizationDemo.MembershipCoreIdentityTests;

public class ProfileParser
{
    public record ParsedProfileProperty(string Name, string? StringValue, byte[]? BinaryValue);

    public static IEnumerable<ParsedProfileProperty> ParseProperties(string map, string stringValues, byte[] binaryValues)
    {
        var chunks = map.TrimEnd(':').Split(':');
        for (var i = 0; i < chunks.Length; i += 4)
        {
            var propertyName = chunks[i];
            var propertyType = chunks[i + 1];
            var valueStartIndex = int.Parse(chunks[i + 2]);
            var valueLength = int.Parse(chunks[i + 3]);

            if (propertyType == "S")
            {
                var value = stringValues.Substring(valueStartIndex, valueLength);
                yield return new ParsedProfileProperty(propertyName, value, null);
            }
            else if (propertyType == "B")
            {
                var value = binaryValues.Skip(valueStartIndex).Take(valueLength).ToArray();
                yield return new ParsedProfileProperty(propertyName, null, value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }

    public static T XmlDeserialize<T>(string value)
    {
        var serializer = new XmlSerializer(typeof(T));
        using var reader = new StringReader(value);
        return (T)serializer.Deserialize(reader)!;
    }

    public static ShoppingCart DeserializeShoppingCart(byte[] binaryValue)
    {
        using var ms = new MemoryStream(binaryValue);
        var document = (BinaryObject)NRBFReader.ReadStream(ms);

        var cart = new ShoppingCart();
        if (document.TryGetValue("<Created>k__BackingField", out var created) && created is DateTime createdDate)
        {
            cart.Created = createdDate;
        }
        if (document.TryGetValue("<LastUpdated>k__BackingField", out var lastUpdated) && lastUpdated is DateTime lastUpdatedDate)
        {
            cart.LastUpdated = lastUpdatedDate;
        }
        if (document.TryGetValue("<Items>k__BackingField", out var items) && items is BinaryObject itemsDictionary
                                                                          && itemsDictionary.TryGetValue("KeyValuePairs", out var keyValuePairs) && keyValuePairs is object[] keyValuePairObjects)
        {
            foreach (var keyValuePairObject in keyValuePairObjects.Cast<BinaryObject>())
            {
                var cartItem = new CartItem();
                if (keyValuePairObject.TryGetValue("key", out var key) && key is string keyString)
                {
                    cartItem.Item = keyString;
                }
                if (keyValuePairObject.TryGetValue("value", out var value) && value is BinaryObject valueObject)
                {
                    if (valueObject.TryGetValue("<Quantity>k__BackingField", out var quantity) && quantity is int quantityValue)
                    {
                        cartItem.Quantity = quantityValue;
                    }
                    if (valueObject.TryGetValue("<Price>k__BackingField", out var price) && price is double priceValue)
                    {
                        cartItem.Price = priceValue;
                    }
                }
                cart.Items.Add(cartItem);
            }
        }
        return cart;
    }
}
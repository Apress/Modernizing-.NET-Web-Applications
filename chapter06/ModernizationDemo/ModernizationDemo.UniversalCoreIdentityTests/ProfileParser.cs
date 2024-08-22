using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ModernizationDemo.UniversalCoreIdentityTests
{

    public class ProfileParser
    {
        public record ParsedProfileProperty(string Name, string? StringValue);

        public static IEnumerable<ParsedProfileProperty> ParseProperties(string map, string stringValues)
        {
            var chunks = map.TrimEnd(':').Split(':');
            for (var i = 0; i < chunks.Length; i += 3)
            {
                var propertyName = chunks[i];
                var valueStartIndex = int.Parse(chunks[i + 1]);
                var valueLength = int.Parse(chunks[i + 2]);

                var value = stringValues.Substring(valueStartIndex, valueLength);
                yield return new ParsedProfileProperty(propertyName, value);
            }
        }

        public static T XmlDeserialize<T>(string value)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var reader = new StringReader(value);
            return (T)serializer.Deserialize(reader)!;
        }

        public static ShoppingCart DeserializeShoppingCart(string stringValue)
        {
            // rewrite assembly name
            var xml = XDocument.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(stringValue)));
            var rootNs = xml.Root.Name.Namespace;

            return new ShoppingCart()
            {
                Created = DateTime.Parse(xml.Root.Element(rootNs + "_x003C_Created_x003E_k__BackingField").Value, CultureInfo.InvariantCulture),
                LastUpdated = DateTime.Parse(xml.Root.Element(rootNs + "_x003C_LastUpdated_x003E_k__BackingField").Value, CultureInfo.InvariantCulture),
                Items = xml.Root
                    .Element(rootNs + "_x003C_Items_x003E_k__BackingField")
                    .Element("KeyValuePairs")
                    .Elements()
                    .Select(i => new CartItem()
                    {
                        Item = i.Element(i.Name.Namespace + "key").Value,
                        Quantity = int.Parse(i.Element(i.Name.Namespace + "value").Element(rootNs +"_x003C_Quantity_x003E_k__BackingField").Value, CultureInfo.InvariantCulture),
                        Price = double.Parse(i.Element(i.Name.Namespace + "value").Element(rootNs + "_x003C_Price_x003E_k__BackingField").Value, CultureInfo.InvariantCulture)
                    })
                    .ToList()
            };
        }
    }
}
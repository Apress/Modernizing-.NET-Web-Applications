namespace ModernizationDemo.IdentityCoreIdentityTests;

public static class Extensions
{
    public static object? HandleDbNull(this object? value)
    {
        return value == DBNull.Value ? null : value;
    }
}
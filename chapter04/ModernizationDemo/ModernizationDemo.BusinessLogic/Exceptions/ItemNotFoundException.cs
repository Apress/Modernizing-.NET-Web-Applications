using System;

namespace ModernizationDemo.BusinessLogic.Exceptions;

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException() : base("The item was not found.")
    {
    }
}
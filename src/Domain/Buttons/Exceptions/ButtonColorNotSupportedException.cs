namespace ButtonShop.Domain.Exceptions;

public sealed class ButtonColorNotSupportedException : Exception
{
    public ButtonColorNotSupportedException(string color)
        : base($"We do not sell such color: {color}")
    {
    }
}

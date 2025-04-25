using ButtonShop.Domain.Buttons.Exceptions;

namespace ButtonShop.Domain.Exceptions;

public sealed class ButtonColorNotSupportedException : DomainException
{
    public ButtonColorNotSupportedException(string color)
        : base($"We do not sell such color: {color}")
    {
    }

    public override string Code => "Domain Exception";
}

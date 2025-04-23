using ButtonShop.Application.Commands;

namespace ButtonShop.Application.Validators;

internal sealed class ShipValidator : AbstractValidator<Ship>
{
    private static string ORDER_EMPTY_CODE = "order_id_empty";
    private static string ORDER_EMPTY_MESSAGE = "OrderId can not be an empty guid";
    public ShipValidator()
    {
        this.RuleFor(request => request.OrderId)
            .NotEmpty()
            .WithErrorCode(ORDER_EMPTY_CODE)
            .WithMessage(ORDER_EMPTY_MESSAGE);

    }
}


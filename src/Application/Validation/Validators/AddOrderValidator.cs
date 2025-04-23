using ButtonShop.Application.Commands;
using ButtonShop.Domain.Entities;

namespace ButtonShop.Application.Validation.Validators;

internal sealed class AddOrderValidator : AbstractValidator<AddOrder>
{
    private static string CUSTOMER_INVALID_NAME_CODE = "customer_name_invalid";
    private static string CUSTOMER_INVALID_NAME_MESSAGE = "Customer name should containe more than two characters";

    private static string ITEM_INVALID_KEY_CODE = "invalid_button_color";
    private static string ITEM_INVALID_KEY_MESSAGE_TEMPLATE = "We sell only:";
    private static string INVALID_ITEM_QUANTITY_CODE = "invalid_button_color";
    private static string INVALID_ITEM_QUANTITY_MESSAGE = "We sell only:";

    private static string INVALID_LONGITUDE_CODE = "invalid_button_color";
    private static string INVALID_LONGITUDE_MESSAGE = "We sell only:";
    private static string INVALID_LATITUDE_CODE = "invalid_button_color";
    private static string INVALID_LATITUDE_MESSAGE = "We sell only";


    private static readonly HashSet<string> allowedKeys = Enum.GetNames(typeof(ButtonColors)).ToHashSet();
    public AddOrderValidator()
    {
        RuleFor(request => request.CustomerName)
            .NotEmpty()
            .MinimumLength(3)
            .WithErrorCode(CUSTOMER_INVALID_NAME_CODE)
            .WithMessage(CUSTOMER_INVALID_NAME_MESSAGE);

        RuleFor(x => x.Items)
            .NotNull()
            .Must(x => x.Count >= 1)
                .WithErrorCode(INVALID_ITEM_QUANTITY_CODE)
                .WithMessage(INVALID_ITEM_QUANTITY_MESSAGE)
            .Must(dict => dict.Keys.All(key => allowedKeys.Contains(key)))
                .WithErrorCode(ITEM_INVALID_KEY_CODE)
                .WithMessage($"{ITEM_INVALID_KEY_MESSAGE_TEMPLATE}: {string.Join(", ", allowedKeys)}");

        RuleFor(x => x.Latitude)
            .NotEmpty()
            .Must(BeValidLatitude)
            .WithErrorCode(INVALID_LATITUDE_CODE)
            .WithMessage(INVALID_LATITUDE_MESSAGE);

        RuleFor(x => x.Longitude)
            .NotEmpty()
            .Must(BeValidLongitude)
            .WithErrorCode(INVALID_LONGITUDE_CODE)
            .WithMessage(INVALID_LONGITUDE_MESSAGE);


    }

    private bool BeValidLatitude(string value)
    {
        return double.TryParse(value, out var result) && result >= -90 && result <= 90;
    }

    private bool BeValidLongitude(string value)
    {
        return double.TryParse(value, out var result) && result >= -180 && result <= 180;
    }
}


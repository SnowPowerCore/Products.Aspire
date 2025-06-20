using FluentValidation;
using Products.PublicApi.BusinessObjects.Dto;

namespace Products.PublicApi.Validation.Dto;

public sealed class ProductRequestValidator : AbstractValidator<ProductRequestDto>
{
    public ProductRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0m);
    }
}
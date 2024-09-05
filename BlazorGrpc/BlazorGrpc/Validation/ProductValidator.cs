using BlazorGrpc.FluentValidation;
using BlazorGrpc.Model;

namespace BlazorGrpc.Validation;

public class ProductValidator : Validator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name.Value, nameof(Product.Name.Value))
            .NotEmpty()
            .Length(1, 10);

        RuleFor(p => p.Price.Value, nameof(Product.Price)).AddRule(p => p.Price.Value > 0m, "Price must be greater than 0.");
    }
}

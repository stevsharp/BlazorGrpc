using BlazorGrpc.FluentValidation;
using BlazorGrpc.Model;

namespace BlazorGrpc.Validation;

public class ProductValidator : Validator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name, nameof(Product.Name))
            .NotEmpty()
            .Length(1, 10);

        RuleFor(p => p.Price, nameof(Product.Price)).AddRule(p => p.Price > 0m, "Price must be greater than 0.");
    }
}

namespace BlazorGrpc.FluentValidation;

public abstract class Validator<T> : IValidator<T> where T : class
{
    private readonly List<Func<T, ValidationResult>> _rules = [];

    public virtual void AddRule(Func<T, ValidationResult> rule)
    {
        _rules.Add(rule);
    }

    public virtual void AddRule(Func<T, ValidationResult> rule, Action action)
    {
        _rules.Add(rule);

        action();
    }

    public ValidationResult Validate(T instance)
    {
        var validationResult = new ValidationResult();

        foreach (var rule in _rules)
        {
            var result = rule(instance);
            if (!result.IsValid)
            {
                validationResult.AddErrors(result.Errors);
            }
        }

        return validationResult;
    }

    public PropertyRule<T, TProperty> RuleFor<TProperty>(Func<T, TProperty> propertyFunc, string propertyName)
        where TProperty : IComparable
    {
        var rule = new PropertyRule<T, TProperty>(propertyFunc, propertyName, this);

        return rule;
    }
}

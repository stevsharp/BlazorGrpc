
namespace BlazorGrpc.FluentValidation;

public interface IValidator<T>
{
    ValidationResult Validate(T instance);
}



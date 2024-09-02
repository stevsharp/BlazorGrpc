using System.Linq.Expressions;

namespace BlazorGrpc.FluentValidation
{
    public class PropertyRule<T, TProperty>(Func<T, TProperty> propertyFunc, string propertyName, Validator<T> validator)
        where T : class
        where TProperty : IComparable
    {
        private readonly Func<T, TProperty> _propertyFunc = propertyFunc;
        private readonly string _propertyName = propertyName;
        private readonly Validator<T> _validator = validator;
        private List<Expression<Func<T, bool>>> _expressions = [];

        public PropertyRule<T, TProperty> NotEmpty()
        {
            _validator.AddRule(instance =>
            {
                var value = _propertyFunc(instance);

                var isValid = value is not null
                        && !value.Equals(default(TProperty));

                var result = new ValidationResult();

                if (!isValid)
                {
                    result.AddError($"{_propertyName} must not be empty.");
                }

                return result;
            });

            return this;



        }

        public PropertyRule<T, TProperty> Length(int min, int max)
        {
            _validator.AddRule(instance =>
            {
                var value = _propertyFunc(instance)?.ToString();
                var isValid = value is not null && value.Length >= min && value.Length <= max;

                var result = new ValidationResult();

                if (!isValid)
                {
                    result.AddError($"{_propertyName} length must be between {min} and {max} characters.");
                }

                return result;
            });

            return this;
        }

        public PropertyRule<T, TProperty> Length(int min, int max, string errorMessage)
        {
            _validator.AddRule(instance =>
            {
                var value = _propertyFunc(instance)?.ToString();
                var isValid = value is not null && value.Length >= min && value.Length <= max;

                var result = new ValidationResult();

                if (!isValid)
                {
                    result.AddError(errorMessage);
                }

                return result;
            });

            return this;
        }

        public PropertyRule<T, TProperty> AddRule(Expression<Func<T, bool>> expression, string errorMessage)
        {
            _validator.AddRule(instance =>
            {
                var compiled = expression.Compile();
                var isValid = compiled.Invoke(instance);

                var result = new ValidationResult();

                if (!isValid)
                {
                    result.AddError(errorMessage);
                }
                return result;
            });

            _expressions.Add(expression);

            return this;
        }

        public PropertyRule<T, TProperty> GreaterThan(TProperty threshold)
        {
            _validator.AddRule(instance =>
            {
                var value = _propertyFunc(instance);

                var result = new ValidationResult();

                if (!(value != null && value.CompareTo(threshold) > 0))
                {
                    result.AddError($"{_propertyName} must be greater than {threshold}.");
                }

                return result;
            });

            return this;
        }
    }
}



namespace BlazorGrpc.FluentValidation
{
    public class ValidationResult
    {
        private readonly List<string> _errors = [];

        public bool IsValid => !_errors.Any();

        public IEnumerable<string> Errors => _errors;

        public void AddError(string error)
        {
            _errors.Add(error);
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            _errors.AddRange(errors);
        }
    }
}



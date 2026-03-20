namespace SoccerPro.Application.Common.Errors
{
    public class Error
    {
        // Common Errors that could occur inside API
        private static readonly string _internalServerError = "intertnalServerError";
        private static readonly string _recoredNotFound = "recoredNotFound";
        private static readonly string _validationError = "ValidationError";
        private static readonly string _forbiddenError = "ForbiddenError";
        private static readonly string _unauthorizedError = "UnauthorizedError";
        private static readonly string _busniessError = "BusniessError";
        private static readonly string _conflictError = "ConflictError"; // ✅ Added

        private readonly string _code;
        private readonly string _message;

        public string Message => _message;
        public string Code => _code;

        public Error(string code, string message)
        {
            _code = code;
            _message = message;
        }

        public static Error None => new(string.Empty, string.Empty);

        public static Error RecoredNotFound(string message) => new(_recoredNotFound, message);
        public static Error InternalServerError(string message) => new(_internalServerError, message);
        public static Error ValidationError(string message) => new(_validationError, message);
        public static Error ForbiddenError(string message) => new(_forbiddenError, message);
        public static Error UnauthorizedError(string message) => new(_unauthorizedError, message);
        public static Error BusniessError(string message) => new(_busniessError, message);
        public static Error ConflictError(string message) => new(_conflictError, message); // ✅ Added
    }
}

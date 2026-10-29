namespace API_with_DOCKER.DesignsPatterns.Results
{
    public enum ResultStatus
    {
        Success,
        ValidationFailure,
        NotFound,
        Conflict,
        Failure
    }
    public class Result<T>
    {
        public ResultStatus Status { get; }
        public T? Value { get; }
        public string? Error { get; }

        public bool IsSuccess => Status == ResultStatus.Success;

        private Result(ResultStatus status, T? value, string? error)
        {
            Status = status;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value)=> new(ResultStatus.Success, value, null);

        public static Result<T> ValidationFailure(string error)=> new(ResultStatus.ValidationFailure, default, error);

        public static Result<T> NotFound(string error) => new(ResultStatus.NotFound, default, error);

        public static Result<T> Conflict(string error)=> new(ResultStatus.Conflict, default, error);

        public static Result<T> Failure(string error)=> new(ResultStatus.Failure, default, error);
    }
}

namespace RealtimeRating.Composition;

public class ComposeResultFailure
{
    public required string Message { get; init; }
    public required IReadOnlyCollection<string> ExceptionMessages { get; init; }
    public required IReadOnlyCollection<string> RollbackExceptionMessages { get; init; }

    public static ComposeResultFailure Create(IReadOnlyCollection<Exception> exceptions, IReadOnlyCollection<Exception> rollbackExceptions)
    {
        return new ComposeResultFailure
        {
            Message = rollbackExceptions.Count != 0 ? "Both participate and rollback exceptions occured" : "Participate exceptions occurred",
            ExceptionMessages = exceptions.Select(x => x.Message).ToArray(),
            RollbackExceptionMessages = rollbackExceptions.Select(x => x.Message).ToArray()
        };
    }
}
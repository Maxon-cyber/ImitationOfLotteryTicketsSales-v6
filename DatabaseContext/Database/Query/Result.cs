namespace DatabaseContext.Database.Query;

public class Result<TData>
    where TData : notnull
{
    public TData? TextResult { get; set; }

    public string? TextError { get; set; }
}
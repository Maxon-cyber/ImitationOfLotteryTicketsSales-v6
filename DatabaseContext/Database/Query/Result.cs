namespace DatabaseContext.Database.Query;

public class Result<TData>
{
    public TData? TextResult { get; set; }

    public string? TextError { get; set; }
}
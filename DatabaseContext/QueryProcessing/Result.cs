namespace DatabaseContext.QueyProcessing;

public class Result<TData>
{
    public TData? TextResult { get; internal set; }

    public string? TextError { get; set; }
}
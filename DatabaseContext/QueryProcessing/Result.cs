namespace DatabaseContext.QueyProcessing;

public class Result<TData>
{
    public TData? Value { get; internal set; }

    public string? TextError { get; set; }
}
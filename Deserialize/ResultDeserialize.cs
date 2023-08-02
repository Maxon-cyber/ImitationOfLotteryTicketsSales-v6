namespace Deserialize;

public class ResultDeserialize<TData>
    where TData : class
{
    public TData? Result { get; set; }
}
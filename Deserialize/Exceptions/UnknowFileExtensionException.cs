using System.Runtime.Serialization;

namespace Deserialize.Exceptions;

[Serializable]
public class UnknowFileExtensionException : Exception
{
	public UnknowFileExtensionException() { }
	public UnknowFileExtensionException(string message) : base(message) { }
	public UnknowFileExtensionException(string message, Exception inner) : base(message, inner) { }
	protected UnknowFileExtensionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
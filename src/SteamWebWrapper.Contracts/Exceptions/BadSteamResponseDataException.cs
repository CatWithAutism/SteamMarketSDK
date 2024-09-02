using System.Runtime.Serialization;
using System.Text.Json;

namespace SteamWebWrapper.Contracts.Exceptions;

public class BadSteamResponseDataException : JsonException
{
	public BadSteamResponseDataException()
	{
	}

	public BadSteamResponseDataException(string? message) : base(message)
	{
	}

	public BadSteamResponseDataException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	public BadSteamResponseDataException(string? message, string? path, long? lineNumber, long? bytePositionInLine) :
		base(message, path, lineNumber, bytePositionInLine)
	{
	}

	public BadSteamResponseDataException(string? message, string? path, long? lineNumber, long? bytePositionInLine,
		Exception? innerException) : base(message, path, lineNumber, bytePositionInLine, innerException)
	{
	}

	protected BadSteamResponseDataException(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}
}
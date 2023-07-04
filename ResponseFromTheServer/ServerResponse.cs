namespace ResponseFromTheServer;

public enum ServerResponse
{
    Ok = 200,
    ConnectionClosed = 333,
    ConnectionIsStable = 220,
    ServerSuccessfullyStarted = 233,
    RequestProcessedSuccessfully = 242,
    NotFound = 404,
    ConnectionIsInterrupted = 405,
    InvalidValue = 406,
}
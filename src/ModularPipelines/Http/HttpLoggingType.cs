namespace ModularPipelines.Http;

[Flags]
public enum HttpLoggingType
{
    Request = 1,
    Response = 2,
    StatusCode = 4,
    Duration = 8,
    None,
}
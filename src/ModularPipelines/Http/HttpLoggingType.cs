using System;

namespace ModularPipelines.Http;

[Flags]
public enum HttpLoggingType
{
    None = 0,
    Request = 1 << 0,
    Response = 1 << 1,
    StatusCode = 1 << 2,
    Duration = 1 << 3,
}
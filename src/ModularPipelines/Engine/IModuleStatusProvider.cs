using ModularPipelines.Enums;

namespace ModularPipelines.Engine;

internal interface IModuleStatusProvider
{
    public Status? GetStatusForModule<T>();
}
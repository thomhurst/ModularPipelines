using ModularPipelines.Models;

namespace ModularPipelines.Engine.Executors.ModuleHandlers;

internal interface IErrorHandler<T>
{
    Task<ModuleResult<T>> Handle(Exception exception);
}
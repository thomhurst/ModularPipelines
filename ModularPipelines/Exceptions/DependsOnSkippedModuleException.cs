using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

public class DependsOnSkippedModuleException : Exception
{
    internal DependsOnSkippedModuleException( ModuleBase currentModule, ModuleBase dependencyModule ) : base( $"{currentModule.GetType().Name} depends on skipped module: {dependencyModule.GetType().Name}" )
    {
    }
}

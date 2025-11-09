using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

/// <summary>
/// Exception thrown when a module exceeds its configured timeout.
/// </summary>
public class ModuleTimeoutException : PipelineException
{
    public ModuleTimeoutException(IModule? module)
        : base($"Module {module?.ModuleType.Name ?? "Unknown"} timed out")
    {
        Module = module;
    }

    public IModule? Module { get; }
}

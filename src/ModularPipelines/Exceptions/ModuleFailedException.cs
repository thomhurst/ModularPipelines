using ModularPipelines.Modules;

namespace ModularPipelines.Exceptions;

public class ModuleFailedException : PipelineException
{
    public ModuleBase Module { get; }

    public ModuleFailedException(ModuleBase module, Exception exception) : base($"The module {module.GetType().Name} has failed.{GetInnerMessage(exception)}", exception)
    {
        Module = module;
    }

    private static string GetInnerMessage(Exception exception)
    {
        if (!string.IsNullOrEmpty(exception.Message))
        {
            return $"{Environment.NewLine}{Environment.NewLine}{exception.Message}";
        }

        return string.Empty;
    }
}
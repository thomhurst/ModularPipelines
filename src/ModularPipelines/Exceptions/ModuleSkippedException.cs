namespace ModularPipelines.Exceptions;

public class ModuleSkippedException : PipelineException
{
    public string ModuleName { get; }

    public ModuleSkippedException(string moduleName) : base($"The module {moduleName} was skipped. No Value available.")
    {
        ModuleName = moduleName;
    }
}

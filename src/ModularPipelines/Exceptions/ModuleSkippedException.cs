namespace ModularPipelines.Exceptions;

[Obsolete("Use pattern matching on ModuleResult<T>.Skipped instead. This exception is no longer thrown by ModuleResult<T>.Value.")]
public class ModuleSkippedException : PipelineException
{
    public string ModuleName { get; }

    public ModuleSkippedException(string moduleName) : base($"The module {moduleName} was skipped. No Value available.")
    {
        ModuleName = moduleName;
    }
}
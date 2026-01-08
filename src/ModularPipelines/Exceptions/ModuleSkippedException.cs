namespace ModularPipelines.Exceptions;

[Obsolete("Use pattern matching on ModuleResult<T>.Skipped instead. The .Value property has been removed; use ValueOrDefault or Match() for safe access.")]
public class ModuleSkippedException : PipelineException
{
    public string ModuleName { get; }

    public ModuleSkippedException(string moduleName) : base($"The module {moduleName} was skipped. No Value available.")
    {
        ModuleName = moduleName;
    }
}
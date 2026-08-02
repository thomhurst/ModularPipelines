namespace ModularPipelines.Engine;

internal sealed class PlanningModuleResultUnavailableException : InvalidOperationException
{
    public PlanningModuleResultUnavailableException(Type moduleType)
        : base($"The result of module '{moduleType.Name}' is unavailable while building a pipeline plan.")
    {
    }
}

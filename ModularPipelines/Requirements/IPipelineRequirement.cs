namespace ModularPipelines.Requirements;

public interface IPipelineRequirement
{
    Task<bool> MustAsync();
}

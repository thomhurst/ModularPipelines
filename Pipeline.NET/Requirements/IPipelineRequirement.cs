namespace Pipeline.NET.Requirements;

public interface IPipelineRequirement
{
    Task<bool> MustAsync();
}
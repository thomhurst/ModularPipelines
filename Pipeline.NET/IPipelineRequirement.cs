namespace Pipeline.NET;

public interface IPipelineRequirement
{
    Task<bool> MustAsync();
}
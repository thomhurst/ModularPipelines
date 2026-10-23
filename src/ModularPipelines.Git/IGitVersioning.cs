namespace ModularPipelines.Git;

public interface IGitVersioning
{
    Task<GitVersionInformation> GetVersioningInformationAsync(CancellationToken cancellationToken = default);
}

namespace ModularPipelines.Build.Helpers;

internal interface IBuildOutputTransport
{
    Task PublishAsync(string repositoryRoot, CancellationToken cancellationToken);

    Task DownloadAsync(string repositoryRoot, CancellationToken cancellationToken);
}

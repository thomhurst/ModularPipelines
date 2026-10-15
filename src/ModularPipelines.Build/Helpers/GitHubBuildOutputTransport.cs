using System.Diagnostics;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed;

namespace ModularPipelines.Build.Helpers;

internal sealed class GitHubBuildOutputTransport(IOptions<DistributedOptions> options) : IBuildOutputTransport
{
    public Task PublishAsync(string repositoryRoot, CancellationToken cancellationToken) =>
        TransferAsync("upload", repositoryRoot, cancellationToken);

    public Task DownloadAsync(string repositoryRoot, CancellationToken cancellationToken) =>
        TransferAsync("download", repositoryRoot, cancellationToken);

    private async Task TransferAsync(string operation, string repositoryRoot, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var startInfo = new ProcessStartInfo("node")
        {
            UseShellExecute = false,
            WorkingDirectory = repositoryRoot,
            CreateNoWindow = true,
        };
        startInfo.ArgumentList.Add(Path.Combine(repositoryRoot, ".github", "actions", "run-pipeline", "artifact.mjs"));
        startInfo.ArgumentList.Add(operation);
        startInfo.ArgumentList.Add(options.Value.RunId);
        startInfo.ArgumentList.Add(repositoryRoot);
        using var process = Process.Start(startInfo)
            ?? throw new InvalidOperationException("Unable to start the GitHub artifact transfer.");
        try
        {
            await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            try
            {
                process.Kill(entireProcessTree: true);
            }
            catch (InvalidOperationException) when (process.HasExited)
            {
                // The transfer may finish while cancellation is being delivered.
            }
            await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false);
            throw;
        }

        if (process.ExitCode != 0)
        {
            throw new IOException($"GitHub build-output artifact {operation} failed (exit {process.ExitCode}).");
        }
    }
}

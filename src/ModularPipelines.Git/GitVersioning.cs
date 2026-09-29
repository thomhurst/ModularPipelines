using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Logging;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

/// <summary>
/// Provides Git versioning information using GitVersion.Tool.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. The <see cref="GetVersioningInformationAsync"/>
/// method can be called concurrently from multiple threads without external synchronization.
/// </para>
/// <para>
/// <b>Synchronization Strategy:</b> Uses a static <see cref="SemaphoreSlim"/> with count 1 as an
/// async mutex. This is required because the operation involves async I/O (tool installation and
/// execution) and regular locks cannot be held across await points. The static semaphore ensures
/// that only one GitVersion tool installation/execution occurs across all instances, preventing
/// race conditions when multiple modules request version information simultaneously.
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class GitVersioning : IGitVersioning
{
    private readonly IGitInformation _gitInformation;
    private readonly ICommandContext _command;
    private readonly IModuleLoggerAccessor _moduleLoggerAccessor;

    private readonly IFileSystemProvider _fileSystemProvider;

    /// <summary>
    /// Async mutex to ensure single-threaded access to GitVersion tool installation and execution.
    /// Static because the cached result and tool installation are shared across all instances.
    /// </summary>
    private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private static GitVersionInformation? _prefetchedGitVersionInformation;

    public GitVersioning(
        IGitInformation gitInformation,
        ICommandContext command,
        IModuleLoggerAccessor moduleLoggerAccessor,
        IFileSystemProvider fileSystemProvider)
    {
        _gitInformation = gitInformation;
        _command = command;
        _moduleLoggerAccessor = moduleLoggerAccessor;
        _fileSystemProvider = fileSystemProvider;
    }

    public async Task<GitVersionInformation> GetVersioningInformationAsync(
        CancellationToken cancellationToken = default)
    {
        await _semaphoreSlim.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (_prefetchedGitVersionInformation != null)
            {
                return _prefetchedGitVersionInformation;
            }

            var repositoryInfo = await _gitInformation.GetInfoAsync(cancellationToken).ConfigureAwait(false)
                ?? throw new InvalidOperationException("Git repository information is unavailable.");

            // The tool is only needed until the version has been computed and cached.
            var temporaryFolder = FolderPath.CreateTemporaryFolder(_fileSystemProvider);
            try
            {
                return _prefetchedGitVersionInformation = await ComputeVersioningInformationAsync(
                        repositoryInfo.Root,
                        temporaryFolder,
                        cancellationToken)
                    .ConfigureAwait(false);
            }
            finally
            {
                TryDeleteTemporaryFolder(temporaryFolder);
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private async Task<GitVersionInformation> ComputeVersioningInformationAsync(
        FolderPath repositoryRoot,
        FolderPath temporaryFolder,
        CancellationToken cancellationToken)
    {
        await _command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("dotnet")
        {
            Arguments =
            [
                "tool",
                "install",
                "--tool-path", temporaryFolder.Path,
                "GitVersion.Tool",
                "--version", "6.*"
            ],
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        await TryWriteConfigurationFileAsync(repositoryRoot, cancellationToken).ConfigureAwait(false);

        var gitVersionOutput = await _command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions(Path.Combine(temporaryFolder, "dotnet-gitversion"))
            {
                Arguments =
                [
                    "/output", "json"
                ],
            },
            new CommandExecutionOptions
            {
                WorkingDirectory = repositoryRoot.Path,
            },
            cancellationToken).ConfigureAwait(false);

        return JsonSerializer.Deserialize<GitVersionInformation>(gitVersionOutput.StandardOutput)!;
    }

    private void TryDeleteTemporaryFolder(FolderPath temporaryFolder)
    {
        try
        {
            temporaryFolder.Delete();
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException)
        {
            _moduleLoggerAccessor.Logger.LogDebug(
                exception,
                "Could not delete GitVersion tool folder {Path}",
                temporaryFolder.Path);
        }
    }

    private async Task TryWriteConfigurationFileAsync(FolderPath root, CancellationToken cancellationToken)
    {
        try
        {
            var file = new FilePath(Path.Combine(root.Path, "GitVersion.yml"));

            if (!file.Exists)
            {
                await file.WriteAsync(
                    """
                    mode: ContinuousDeployment
                    strategies:
                      - Mainline
                    """,
                    cancellationToken).ConfigureAwait(false);
            }
        }
        catch (Exception e) when (e is not (OperationCanceledException or OutOfMemoryException or StackOverflowException))
        {
            _moduleLoggerAccessor.Logger.LogWarning(e, "Error defining GitVersion.yml configuration");
        }
    }
}

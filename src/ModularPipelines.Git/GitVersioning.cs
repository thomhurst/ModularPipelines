using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Git.Options;
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
/// <b>Synchronization Strategy:</b> An instance <see cref="SemaphoreSlim"/> serializes tool
/// installation and execution until a result is cached. Separate instances have independent
/// results and temporary tool folders, so pipelines for different repositories do not share versions.
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class GitVersioning(
    IGitInformation gitInformation,
    ICommandContext command,
    IModuleLoggerAccessor moduleLoggerAccessor,
    IFileSystemProvider fileSystemProvider,
    IOptions<GitVersioningOptions> options) : IGitVersioning
{
    private readonly IGitInformation _gitInformation = gitInformation;
    private readonly ICommandContext _command = command;
    private readonly IModuleLoggerAccessor _moduleLoggerAccessor = moduleLoggerAccessor;

    private readonly IFileSystemProvider _fileSystemProvider = fileSystemProvider;
    private readonly string _toolVersion = options.Value.ToolVersion;

    /// <summary>
    /// Async mutex to ensure single-threaded access to GitVersion tool installation and execution.
    /// Each instance owns its cached result and temporary tool installation.
    /// </summary>
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private GitVersionInformation? _prefetchedGitVersionInformation;

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
                "--version", _toolVersion
            ],
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        var arguments = new List<string> { "/output", "json", "/nocache" };
        if (!HasRepositoryConfiguration(repositoryRoot))
        {
            var configuration = temporaryFolder.GetFile("GitVersion.yml");
            await configuration.WriteAsync(
                """
                mode: ContinuousDeployment
                strategies:
                  - Mainline
                """,
                cancellationToken).ConfigureAwait(false);
            arguments.AddRange(["/config", configuration.Path]);
        }

        var gitVersionOutput = await _command.ExecuteCommandLineToolAsync(
            new CommandLineToolOptions(Path.Combine(temporaryFolder, "dotnet-gitversion"))
            {
                Arguments = arguments,
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
            var logger = _moduleLoggerAccessor.Logger;
            if (logger.IsEnabled(LogLevel.Debug))
            {
                logger.LogDebug(
                    exception,
                    "Could not delete GitVersion tool folder {Path}",
                    temporaryFolder.Path);
            }
        }
    }

    private bool HasRepositoryConfiguration(FolderPath root)
    {
        // Match GitVersion's case-insensitive discovery, including on case-sensitive file systems.
        string[] configurationNames = ["GitVersion.yml", "GitVersion.yaml", ".GitVersion.yml", ".GitVersion.yaml"];
        return _fileSystemProvider.EnumerateFiles(root.Path, "*", SearchOption.TopDirectoryOnly)
            .Any(path => configurationNames.Contains(Path.GetFileName(path), StringComparer.OrdinalIgnoreCase));
    }
}

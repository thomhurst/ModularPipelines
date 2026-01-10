using System.Text.Json;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Models;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Git;

/// <summary>
/// Provides Git versioning information using GitVersion.Tool.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. The <see cref="GetGitVersioningInformation"/>
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
    private readonly ICommand _command;
    private readonly IModuleLoggerProvider _moduleLoggerProvider;

    private readonly Folder _temporaryFolder;

    /// <summary>
    /// Async mutex to ensure single-threaded access to GitVersion tool installation and execution.
    /// Static because the cached result and tool installation are shared across all instances.
    /// </summary>
    private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private static GitVersionInformation? _prefetchedGitVersionInformation;

    public GitVersioning(IFileSystemContext fileSystemContext, IGitInformation gitInformation, ICommand command, IModuleLoggerProvider moduleLoggerProvider)
    {
        _gitInformation = gitInformation;
        _command = command;
        _moduleLoggerProvider = moduleLoggerProvider;
        _temporaryFolder = fileSystemContext.CreateTemporaryFolder();
    }

    public async Task<GitVersionInformation> GetGitVersioningInformation()
    {
        await _semaphoreSlim.WaitAsync().ConfigureAwait(false);

        try
        {
            if (_prefetchedGitVersionInformation != null)
            {
                return _prefetchedGitVersionInformation;
            }

            await _command.ExecuteCommandLineTool(new GenericCommandLineToolOptions("dotnet")
            {
                Arguments =
                [
                    "tool",
                    "install",
                    "--tool-path", _temporaryFolder.Path,
                    "GitVersion.Tool",
                    "--version", "6.*"
                ],
            }).ConfigureAwait(false);

            await TryWriteConfigurationFile().ConfigureAwait(false);

            var gitVersionOutput = await _command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions(Path.Combine(_temporaryFolder, "dotnet-gitversion"))
                {
                    Arguments =
                    [
                        "/output", "json"
                    ],
                },
                new CommandExecutionOptions
                {
                    WorkingDirectory = _gitInformation.Root.Path,
                }).ConfigureAwait(false);

            return _prefetchedGitVersionInformation ??=
                JsonSerializer.Deserialize<GitVersionInformation>(gitVersionOutput.StandardOutput)!;
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private async Task TryWriteConfigurationFile()
    {
        try
        {
            var file = new File(Path.Combine(_gitInformation.Root.Path, "GitVersion.yml"));

            if (!file.Exists)
            {
                await file.WriteAsync(
                    """
                    mode: ContinuousDeployment
                    strategies:
                      - Mainline
                    """
                ).ConfigureAwait(false);
            }
        }
        catch (Exception e) when (e is not (OutOfMemoryException or StackOverflowException))
        {
            _moduleLoggerProvider.GetLogger().LogWarning(e, "Error defining GitVersion.yml configuration");
        }
    }
}
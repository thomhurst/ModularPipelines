using ModularPipelines.Options;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides generic software installation capabilities.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="InstallersContext"/> class.
/// </remarks>
/// <param name="command">The command context.</param>
/// <param name="downloader">The downloader context.</param>
/// <param name="workingDirectory">The pipeline working directory.</param>
internal class InstallersContext(
    ICommandContext command,
    IDownloaderContext downloader,
    PipelineWorkingDirectory workingDirectory) : IInstallersContext
{
    private readonly ICommandContext _command = command;
    private readonly IDownloaderContext _downloader = downloader;
    private readonly PipelineWorkingDirectory _workingDirectory = workingDirectory;

    /// <inheritdoc />
    public virtual async Task<CommandResult> InstallAsync(
        InstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var path = _workingDirectory.ResolvePath(options.Path);
        var arguments = options.Arguments ?? [];
        CommandLineToolOptions commandOptions;
        CommandExecutionOptions? executionOptions = null;
        if (OperatingSystem.IsWindows() && Path.GetExtension(path).Equals(".msi", StringComparison.OrdinalIgnoreCase))
        {
            commandOptions = new CommandLineToolOptions("msiexec")
            {
                Arguments = ["/i", path, .. arguments],
            };
            executionOptions = new CommandExecutionOptions
            {
                SuccessExitCodes = [0, 1641, 3010],
            };
        }
        else if (Path.GetExtension(path).Equals(".ps1", StringComparison.OrdinalIgnoreCase))
        {
            commandOptions = new CommandLineToolOptions("pwsh")
            {
                Arguments = ["-NoLogo", "-NoProfile", "-NonInteractive", "-File", path, .. arguments],
            };
        }
        else
        {
            if (!OperatingSystem.IsWindows())
            {
                File.SetUnixFileMode(path, File.GetUnixFileMode(path) | UnixFileMode.UserExecute);
            }

            commandOptions = new CommandLineToolOptions(path)
            {
                Arguments = arguments,
            };
        }

        return await _command.ExecuteCommandLineToolAsync(commandOptions, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual async Task<CommandResult> InstallFromWebAsync(
        WebInstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        var file = await _downloader.DownloadFileAsync(
            new DownloadFileOptions(options.DownloadUri),
            cancellationToken).ConfigureAwait(false);

        try
        {
            return await InstallAsync(new InstallerOptions(file.Path)
            {
                Arguments = options.Arguments,
            }, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            file.Delete();
        }
    }
}

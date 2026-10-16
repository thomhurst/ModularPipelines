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
internal class InstallersContext(
    ICommandContext command,
    IDownloaderContext downloader) : IInstallersContext
{
    private readonly ICommandContext _command = command;
    private readonly IDownloaderContext _downloader = downloader;

    /// <inheritdoc />
    public virtual async Task<CommandResult> InstallAsync(
        InstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        if (!OperatingSystem.IsWindows())
        {
            File.SetUnixFileMode(options.Path, File.GetUnixFileMode(options.Path) | UnixFileMode.UserExecute);
        }

        return await _command.ExecuteCommandLineToolAsync(new CommandLineToolOptions(options.Path)
        {
            Arguments = options.Arguments ?? [],
        }, null, cancellationToken).ConfigureAwait(false);
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

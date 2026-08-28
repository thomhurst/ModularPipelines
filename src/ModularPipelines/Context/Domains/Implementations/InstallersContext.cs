using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides generic software installation capabilities.
/// </summary>
internal class InstallersContext : IInstallersContext
{
    private readonly ICommandContext _command;
    private readonly IDownloaderContext _downloader;
    private readonly IBashContext _bash;

    /// <summary>
    /// Initializes a new instance of the <see cref="InstallersContext"/> class.
    /// </summary>
    /// <param name="command">The command context.</param>
    /// <param name="downloader">The downloader context.</param>
    /// <param name="bash">The Bash context.</param>
    public InstallersContext(
        ICommandContext command,
        IDownloaderContext downloader,
        IBashContext bash)
    {
        _command = command;
        _downloader = downloader;
        _bash = bash;
    }

    /// <inheritdoc />
    public virtual async Task<CommandResult> InstallAsync(
        InstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        if (OperatingSystem.IsWindows())
        {
            return await _command.ExecuteCommandLineToolAsync(new CommandLineToolOptions(options.Path)
            {
                Arguments = options.Arguments ?? Array.Empty<string>(),
            }, null, cancellationToken).ConfigureAwait(false);
        }

        var escapedPath = ShellArgumentEscaper.Escape(options.Path);
        await _bash.RunAsync(
            new BashCommandOptions($"chmod u+x {escapedPath}"),
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return await _bash.RunFileAsync(
            new BashFileOptions(options.Path),
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public virtual async Task<CommandResult> InstallFromWebAsync(
        WebInstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        var file = await _downloader.DownloadFileAsync(
            new DownloadFileOptions(options.DownloadUri),
            cancellationToken).ConfigureAwait(false);

        return await InstallAsync(new InstallerOptions(file.Path)
        {
            Arguments = options.Arguments,
        }, cancellationToken).ConfigureAwait(false);
    }
}

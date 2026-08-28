using ModularPipelines.Context.Domains.Network;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public class FileInstaller : IFileInstaller
{
    private readonly ICommandContext _command;
    private readonly IDownloaderContext _downloader;
    private readonly IBashContext _bash;

    public FileInstaller(ICommandContext command, IDownloaderContext downloader, IBashContext bash)
    {
        _command = command;
        _downloader = downloader;
        _bash = bash;
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> InstallFromFileAsync(InstallerOptions options,
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
        await _bash.CommandAsync(new BashCommandOptions($"chmod u+x {escapedPath}"), cancellationToken).ConfigureAwait(false);

        return await _bash.FromFileAsync(new BashFileOptions(options.Path), cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<CommandResult> InstallFromWebAsync(WebInstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        var file = await _downloader.DownloadFileAsync(new DownloadFileOptions(options.DownloadUri), cancellationToken).ConfigureAwait(false);

        return await InstallFromFileAsync(new InstallerOptions(file.Path)
        {
            Arguments = options.Arguments,
        }, cancellationToken).ConfigureAwait(false);
    }
}

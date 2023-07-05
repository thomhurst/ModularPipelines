using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public class Installer : IInstaller
{
    private readonly ICommand _command;
    private readonly IDownloader _downloader;

    public Installer(ICommand command, IDownloader downloader)
    {
        _command = command;
        _downloader = downloader;
    }

    public Task<CommandResult> InstallFromFileAsync(InstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(new CommandLineToolOptions(options.Path)
        {
            Arguments = options.Arguments ?? Array.Empty<string>()
        }, cancellationToken);
    }

    public async Task<CommandResult> InstallFromWebAsync(WebInstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        var file = await _downloader.DownloadFileAsync(new DownloadOptions(options.DownloadUri), cancellationToken);

        return await InstallFromFileAsync(new InstallerOptions(file.Path)
        {
            Arguments = options.Arguments
        }, cancellationToken);
    }
    

}
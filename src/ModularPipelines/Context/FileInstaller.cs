using System.Runtime.InteropServices;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public class FileInstaller : IFileInstaller
{
    private readonly ICommand _command;
    private readonly IDownloader _downloader;
    private readonly IBash _bash;

    public FileInstaller(ICommand command, IDownloader downloader, IBash bash)
    {
        _command = command;
        _downloader = downloader;
        _bash = bash;
    }

    public async Task<CommandResult> InstallFromFileAsync(InstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        if(OperatingSystem.IsWindows)
        {
            return await _command.ExecuteCommandLineTool(new CommandLineToolOptions(options.Path)
            {
                Arguments = options.Arguments ?? Array.Empty<string>()
            }, cancellationToken);
        }

        await _bash.Command(new BashCommandOptions($"chmod u+x {options.Path}"), cancellationToken);

        return await _bash.FromFile(new BashFileOptions(options.Path), cancellationToken);
    }

    public async Task<CommandResult> InstallFromWebAsync(WebInstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        var file = await _downloader.DownloadFileAsync(new DownloadFileOptions(options.DownloadUri), cancellationToken);

        return await InstallFromFileAsync(new InstallerOptions(file.Path)
        {
            Arguments = options.Arguments
        }, cancellationToken);
    }
}
using System.Text.Json;
using CliWrap.Buffered;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Command.Extensions;
using ModularPipelines.Command.Options;
using ModularPipelines.Context;
using ModularPipelines.Installer.Options;

namespace ModularPipelines.Installer;

public class Installer : IInstaller
{
    public IModuleContext Context { get; }

    [ActivatorUtilitiesConstructor]
    internal Installer(IModuleContext context)
    {
        Context = context;
    }

    public Task<BufferedCommandResult> InstallFromFile(InstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        return Context.Command().UsingCommandLineTool(new CommandLineToolOptions(options.Path)
        {
            Arguments = options.Arguments ?? Array.Empty<string>()
        }, cancellationToken);
    }

    public async Task<BufferedCommandResult> InstallFromWeb(WebInstallerOptions options,
        CancellationToken cancellationToken = default)
    {
        var httpClient = Context.Get<HttpClient>()!;

        await using var stream = await httpClient.GetStreamAsync(options.DownloadUri, cancellationToken);

        var filePathToSave = Path.GetTempFileName() + Guid.NewGuid() + GetExtension(options.DownloadUri.AbsoluteUri);

        await using (var newFile = File.Create(filePathToSave))
        {
            await stream.CopyToAsync(newFile, cancellationToken);
        }

        return await InstallFromFile(new InstallerOptions(filePathToSave)
        {
            Arguments = options.Arguments
        }, cancellationToken);
    }
    
    private string GetExtension(string downloadUri)
    {
        if (Path.HasExtension(downloadUri))
        {
            return Path.GetExtension(downloadUri);
        }

        return string.Empty;
    }
}
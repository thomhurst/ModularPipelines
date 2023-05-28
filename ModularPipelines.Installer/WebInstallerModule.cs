using CliWrap;
using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Installer;

public abstract class WebInstallerModule : Module<BufferedCommandResult>
{
    protected WebInstallerModule(IModuleContext context) : base(context)
    {
    }

    protected abstract Uri DownloadUri { get; }
    public virtual string[]? InstallerArguments { get; }

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var httpClient = Context.Get<HttpClient>()!;

        await using var stream = await httpClient.GetStreamAsync(DownloadUri, cancellationToken);

        var filePathToSave = Path.GetTempFileName() + Guid.NewGuid() + GetExtension();

        await using (var newFile = File.Create(filePathToSave))
        {
            await stream.CopyToAsync(newFile, cancellationToken);
        }

        return await Cli.Wrap(filePathToSave)
            .WithArguments(InstallerArguments ?? Array.Empty<string>())
            .ExecuteBufferedAsync(cancellationToken: cancellationToken);
    }

    private string GetExtension()
    {
        if (Path.HasExtension(DownloadUri.ToString()))
        {
            return Path.GetExtension(DownloadUri.ToString());
        }

        return string.Empty;
    }
}
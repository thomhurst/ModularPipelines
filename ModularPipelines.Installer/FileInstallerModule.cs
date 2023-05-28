using CliWrap;
using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Installer;

public abstract class FileInstallerModule : Module<BufferedCommandResult>
{
    public FileInstallerModule(IModuleContext context) : base(context)
    {
    }
    
    public abstract string FilePath { get; }
    public virtual string[]? InstallerArguments { get; }

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await Cli.Wrap(FilePath)
            .WithArguments(InstallerArguments ?? Array.Empty<string>())
            .ExecuteBufferedAsync(cancellationToken: cancellationToken);
    }
}
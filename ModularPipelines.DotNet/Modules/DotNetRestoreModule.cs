using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.DotNet.Modules;

public abstract class DotNetRestoreModule : Module<BufferedCommandResult>
{
    protected DotNetRestoreModule(IModuleContext context) : base(context)
    {
    }

    protected abstract DotNetModuleOptions Options { get; }

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var internalDotNetCommandModule = new ExternalRunnableDotNetCommandModule(Context, new DotNetCommandModuleOptions
        {
            Command = new[] {"restore"},
            ExtraArguments = Options.ExtraArguments,
            TargetPath = Options.TargetPath,
            WorkingDirectory = Options.WorkingDirectory
        });

        await internalDotNetCommandModule.StartProcessingModule();
        
        return await internalDotNetCommandModule;
    }
}
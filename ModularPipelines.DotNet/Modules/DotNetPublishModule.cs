using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.DotNet.Modules;

public abstract class DotNetPublishModule : Module<BufferedCommandResult>
{
    protected DotNetPublishModule(IModuleContext context) : base(context)
    {
    }

    protected abstract DotNetModuleOptions Options { get; }

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var internalDotNetCommandModule = new ExternalRunnableDotNetCommandModule(Context, new DotNetCommandModuleOptions()
        {
            Command = new[] {"publish"},
            ExtraArguments = Options.ExtraArguments,
            TargetPath = Options.TargetPath,
            WorkingDirectory = Options.WorkingDirectory
        });
        
        await internalDotNetCommandModule.StartProcessingModule();

        return await internalDotNetCommandModule;
    }
}
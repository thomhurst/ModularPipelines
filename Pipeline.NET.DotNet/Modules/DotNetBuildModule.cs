using CliWrap.Buffered;
using Pipeline.NET.Context;
using Pipeline.NET.DotNet.Options;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.DotNet.Modules;

public abstract class DotNetBuildModule : Module<BufferedCommandResult>
{
    protected DotNetBuildModule(IModuleContext context) : base(context)
    {
    }

    protected abstract DotNetModuleOptions Options { get; }

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var internalDotNetCommandModule = new ExternalRunnableDotNetCommandModule(Context, new DotNetCommandModuleOptions()
        {
            Command = new[] {"build"},
            ExtraArguments = Options.ExtraArguments,
            TargetPath = Options.TargetPath,
            WorkingDirectory = Options.WorkingDirectory
        });
        
        await internalDotNetCommandModule.StartProcessingModule();

        return await internalDotNetCommandModule;
    }
}
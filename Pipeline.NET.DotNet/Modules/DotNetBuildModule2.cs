using Pipeline.NET.DotNet.Options;

namespace Pipeline.NET.DotNet.Modules;

public abstract class DotNetRestoreModule : Module
{
    protected DotNetRestoreModule(IModuleContext context) : base(context)
    {
    }

    public abstract DotNetModuleOptions Options { get; }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await new InternalDotNetCommandModule(Context, new DotNetCommandModuleOptions()
        {
            Command = "restore",
            AssertSuccess = Options.AssertSuccess,
            ExtraArguments = Options.ExtraArguments,
            ProjectOrSolutionPath = Options.ProjectOrSolutionPath,
            WorkingDirectory = Options.WorkingDirectory
        }).StartProcessingModule();
    }
}
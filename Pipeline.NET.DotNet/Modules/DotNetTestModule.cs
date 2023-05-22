using Pipeline.NET.DotNet.Options;

namespace Pipeline.NET.DotNet.Modules;

public abstract class DotNetTestModule : Module
{
    protected DotNetTestModule(IModuleContext context) : base(context)
    {
    }

    public abstract DotNetModuleOptions Options { get; }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        return await new InternalDotNetCommandModule(Context, new DotNetCommandModuleOptions()
        {
            Command = "test",
            AssertSuccess = Options.AssertSuccess,
            ExtraArguments = Options.ExtraArguments,
            ProjectOrSolutionPath = Options.ProjectOrSolutionPath,
            WorkingDirectory = Options.WorkingDirectory
        }).StartProcessingModule();
    }
}
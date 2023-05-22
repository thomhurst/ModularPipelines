namespace Pipeline.NET.UnitTests;

public class OneSecondModule : Module
{
    public OneSecondModule(IModuleContext moduleContext) : base(moduleContext)
    {
    }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
        return new ModuleResult();
    }
}
namespace Pipeline.NET.UnitTests;

public class FiveSecondModule : Module
{
    public FiveSecondModule(IModuleContext moduleContext) : base(moduleContext)
    {
    }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        return new ModuleResult();
    }
}
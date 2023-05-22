namespace Pipeline.NET.UnitTests;

public class ThreeSecondModule : Module
{
    public ThreeSecondModule(IModuleContext moduleContext) : base(moduleContext)
    {
    }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);
        return new ModuleResult();
    }
}
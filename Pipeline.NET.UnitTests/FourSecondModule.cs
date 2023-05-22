namespace Pipeline.NET.UnitTests;

public class FourSecondModule : Module
{
    public FourSecondModule(IModuleContext moduleContext) : base(moduleContext)
    {
    }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(4), cancellationToken);
        return new ModuleResult();
    }
}
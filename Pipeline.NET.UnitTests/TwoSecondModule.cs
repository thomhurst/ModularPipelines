namespace Pipeline.NET.UnitTests;

public class TwoSecondModule : Module
{
    public TwoSecondModule(IModuleContext moduleContext) : base(moduleContext)
    {
    }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        return new ModuleResult();
    }
}
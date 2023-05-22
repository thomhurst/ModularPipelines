namespace Pipeline.NET.UnitTests;

public class OneSecondModuleDependentOnFiveSecondModule : OneSecondModule
{
    public OneSecondModuleDependentOnFiveSecondModule(IModuleContext moduleContext) : base(moduleContext)
    {
        DependsOn<FiveSecondModule>();
    }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var fiveSecondModule = await GetModuleAsync<FiveSecondModule>();
        return await base.ExecuteAsync(cancellationToken);
    }
}
namespace Pipeline.NET.UnitTests;

public class DependencyConflictModule1 : Module
{
    public DependencyConflictModule1(IModuleContext moduleContext) : base(moduleContext)
    {
        DependsOn<DependencyConflictModule2>();
    }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return null;
    }
}

public class DependencyConflictModule2 : Module
{
    public DependencyConflictModule2(IModuleContext moduleContext) : base(moduleContext)
    {
        DependsOn<DependencyConflictModule1>();
    }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return null;
    }
}
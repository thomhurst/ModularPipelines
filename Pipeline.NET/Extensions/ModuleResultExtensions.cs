namespace Pipeline.NET.Extensions;

public static class ModuleResultExtensions
{
    public static ModuleResult ToModuleResult(this object obj)
    {
        return ModuleResult.From(obj);
    }
}
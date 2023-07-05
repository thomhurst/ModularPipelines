namespace ModularPipelines.Modules;

public abstract class SubModuleBase
{
    public Type ParentModule { get; }

    internal readonly string Name;
    internal TimeSpan Duration { get; set; }
    internal DateTimeOffset StartTime { get; set; }
    internal DateTimeOffset EndTime { get; set; }

    internal SubModuleBase( Type parentModule, string name )
    {
        ParentModule = parentModule;
        Name = name;
    }
}

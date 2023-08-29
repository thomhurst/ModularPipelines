namespace ModularPipelines.Modules;

public abstract class SubModuleBase
{
    public Type ParentModule { get; }

    public string Name { get; }
    internal TimeSpan Duration { get; set; }
    internal DateTimeOffset StartTime { get; set; }
    internal DateTimeOffset EndTime { get; set; }

    private protected SubModuleBase(Type parentModule, string name)
    {
        ParentModule = parentModule;
        Name = name;
    }
}

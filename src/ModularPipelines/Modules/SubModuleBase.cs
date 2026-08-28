using ModularPipelines.Enums;

namespace ModularPipelines.Modules;

internal abstract class SubModuleBase
{
    public Type ParentModule { get; }

    public string Name { get; }

    internal ModuleStatus Status { get; set; } = ModuleStatus.NotStarted;

    internal TimeSpan Duration { get; set; }

    internal DateTimeOffset StartTime { get; set; }

    internal DateTimeOffset EndTime { get; set; }

    private protected SubModuleBase(Type parentModule, string name)
    {
        ParentModule = parentModule;
        Name = name;
    }
}

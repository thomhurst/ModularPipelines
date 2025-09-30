namespace ModularPipelines.Distributed.Attributes;

/// <summary>
/// Abstract base class for attributes that define scheduling requirements for modules in distributed execution.
/// Unlike RunConditionAttribute (which skips modules locally), these attributes guide the scheduler
/// in assigning modules to workers with matching capabilities.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public abstract class ModuleRequirementAttribute : Attribute
{
}

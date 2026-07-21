namespace ModularPipelines.Attributes;

/// <summary>
/// Declares that a module requires a specific capability to execute.
/// In distributed mode, the module will only be assigned to workers that advertise this capability.
/// Multiple attributes create AND logic (all capabilities required).
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RequiresCapabilityAttribute : Attribute
{
    public RequiresCapabilityAttribute(string capability)
    {
        Capability = capability;
    }

    public string Capability { get; }
}

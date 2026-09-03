namespace ModularPipelines.Attributes;

/// <summary>
/// Declares capabilities that a module requires to execute.
/// In distributed mode, the module will only be assigned to workers that advertise every capability.
/// Multiple attributes and multiple values within one attribute both create AND logic.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RequiresCapabilityAttribute : Attribute
{
    public RequiresCapabilityAttribute(params string[] capabilities)
    {
        ArgumentNullException.ThrowIfNull(capabilities);
        if (capabilities.Length == 0 || capabilities.Any(string.IsNullOrWhiteSpace))
        {
            throw new ArgumentException(
                "At least one non-empty capability is required.",
                nameof(capabilities));
        }

        Capabilities = Array.AsReadOnly([.. capabilities]);
    }

    public IReadOnlyList<string> Capabilities { get; }
}

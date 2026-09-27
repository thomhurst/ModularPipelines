namespace ModularPipelines.Attributes;

/// <summary>
/// Declares alternative capabilities for a module. In distributed mode, the module will only be
/// assigned to workers that advertise at least one of them. When the module would run locally
/// without any of them, it is skipped.
/// Multiple attributes create AND logic between their alternatives.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RequiresAnyCapabilityAttribute : Attribute
{
    public RequiresAnyCapabilityAttribute(params string[] capabilities)
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

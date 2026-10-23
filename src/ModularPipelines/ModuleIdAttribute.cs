namespace ModularPipelines;

/// <summary>
/// Overrides the stable identifier used for module selection, distributed wires, and cache fingerprints.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class ModuleIdAttribute : Attribute
{
    /// <summary>
    /// Initializes a module identifier override.
    /// </summary>
    public ModuleIdAttribute(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        Id = id;
    }

    /// <summary>
    /// Gets the stable identifier override.
    /// </summary>
    public string Id { get; }
}

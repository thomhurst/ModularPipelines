namespace ModularPipelines.Distributed;

public class DistributedOptions
{
    public bool Enabled { get; set; }

    public int InstanceIndex { get; set; }

    public int TotalInstances { get; set; } = 1;

    public IReadOnlyList<string> Capabilities { get; set; } = [];

    public int CapabilityTimeoutSeconds { get; set; } = 300;

    public bool AutoDetectOsCapability { get; set; } = true;

    /// <summary>
    /// Gets or sets the default timeout in seconds for waiting for a distributed module result.
    /// Applied when a module has no explicit Timeout configured. 0 = no timeout (wait forever).
    /// </summary>
    public int ModuleResultTimeoutSeconds { get; set; }
}

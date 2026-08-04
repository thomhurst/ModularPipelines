namespace ModularPipelines.Distributed;

public class DistributedOptions
{
    public bool Enabled { get; set; }

    public int InstanceIndex { get; set; }

    public int TotalInstances { get; set; } = 1;

    /// <summary>
    /// Gets or sets an identifier shared by every process in this pipeline execution.
    /// Coordinators should populate this from their execution-scoping identifier.
    /// </summary>
    public string? ExecutionIdentifier { get; set; }

    public IReadOnlyList<string> Capabilities { get; set; } = [];

    public int CapabilityTimeoutSeconds { get; set; } = 300;

    public bool AutoDetectOsCapability { get; set; } = true;

    /// <summary>
    /// Gets or sets the default timeout in seconds for waiting for a distributed module result.
    /// Defaults to 2700 seconds (45 minutes) and applies when a module has no explicit Timeout configured.
    /// Set to 0 to wait indefinitely.
    /// </summary>
    public int ModuleResultTimeoutSeconds { get; set; } = 2700;
}

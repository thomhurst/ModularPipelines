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
    public string? RunIdentifier { get; set; }

    public IReadOnlyList<string> Capabilities { get; set; } = [];

    public TimeSpan CapabilityTimeout { get; set; } = TimeSpan.FromMinutes(5);

    public bool AutoDetectOsCapability { get; set; } = true;

    /// <summary>
    /// Gets or sets the default timeout for waiting for a distributed module result.
    /// Defaults to 45 minutes and applies when a module has no explicit Timeout configured.
    /// Set to <see cref="TimeSpan.Zero"/> to wait indefinitely.
    /// </summary>
    public TimeSpan ModuleResultTimeout { get; set; } = TimeSpan.FromMinutes(45);
}

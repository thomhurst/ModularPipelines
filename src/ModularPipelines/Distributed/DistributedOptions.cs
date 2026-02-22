namespace ModularPipelines.Distributed;

public class DistributedOptions
{
    public bool Enabled { get; set; }

    public int InstanceIndex { get; set; }

    public int TotalInstances { get; set; } = 1;

    public IList<string> Capabilities { get; set; } = new List<string>();

    public int HeartbeatIntervalSeconds { get; set; } = 10;

    public int HeartbeatTimeoutSeconds { get; set; } = 30;

    public int CapabilityTimeoutSeconds { get; set; } = 300;

    public bool AutoDetectOsCapability { get; set; } = true;
}

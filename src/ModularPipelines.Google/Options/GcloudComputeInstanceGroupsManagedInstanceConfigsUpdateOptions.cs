using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "instance-configs", "update")]
public record GcloudComputeInstanceGroupsManagedInstanceConfigsUpdateOptions(
[property: CliArgument] string Name,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliOption("--instance-update-minimal-action")]
    public string? InstanceUpdateMinimalAction { get; set; }

    [CliOption("--remove-stateful-disks")]
    public string[]? RemoveStatefulDisks { get; set; }

    [CliOption("--remove-stateful-external-ips")]
    public string[]? RemoveStatefulExternalIps { get; set; }

    [CliOption("--remove-stateful-internal-ips")]
    public string[]? RemoveStatefulInternalIps { get; set; }

    [CliOption("--remove-stateful-metadata")]
    public string[]? RemoveStatefulMetadata { get; set; }

    [CliOption("--stateful-disk")]
    public string[]? StatefulDisk { get; set; }

    [CliOption("--stateful-external-ip")]
    public string[]? StatefulExternalIp { get; set; }

    [CliOption("--stateful-internal-ip")]
    public string[]? StatefulInternalIp { get; set; }

    [CliOption("--stateful-metadata")]
    public IEnumerable<KeyValue>? StatefulMetadata { get; set; }

    [CliFlag("--update-instance")]
    public bool? UpdateInstance { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}
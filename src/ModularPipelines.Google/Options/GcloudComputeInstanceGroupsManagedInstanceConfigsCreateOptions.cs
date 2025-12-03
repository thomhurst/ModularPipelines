using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "instance-configs", "create")]
public record GcloudComputeInstanceGroupsManagedInstanceConfigsCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliOption("--instance-update-minimal-action")]
    public string? InstanceUpdateMinimalAction { get; set; }

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
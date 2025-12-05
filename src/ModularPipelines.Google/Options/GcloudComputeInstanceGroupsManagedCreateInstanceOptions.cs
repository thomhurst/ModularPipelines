using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "create-instance")]
public record GcloudComputeInstanceGroupsManagedCreateInstanceOptions(
[property: CliArgument] string Name,
[property: CliOption("--instance")] string Instance
) : GcloudOptions
{
    [CliOption("--stateful-disk")]
    public string[]? StatefulDisk { get; set; }

    [CliOption("--stateful-external-ip")]
    public string[]? StatefulExternalIp { get; set; }

    [CliOption("--stateful-internal-ip")]
    public string[]? StatefulInternalIp { get; set; }

    [CliOption("--stateful-metadata")]
    public IEnumerable<KeyValue>? StatefulMetadata { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}
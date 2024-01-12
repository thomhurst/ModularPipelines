using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "service-connection-policies", "update")]
public record GcloudNetworkConnectivityServiceConnectionPoliciesUpdateOptions(
[property: PositionalArgument] string ServiceConnectionPolicy,
[property: CommandSwitch("--subnets")] string[] Subnets,
[property: CommandSwitch("--psc-connection-limit")] string PscConnectionLimit
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}
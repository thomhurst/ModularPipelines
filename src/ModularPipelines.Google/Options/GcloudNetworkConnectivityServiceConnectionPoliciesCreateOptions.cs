using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "service-connection-policies", "create")]
public record GcloudNetworkConnectivityServiceConnectionPoliciesCreateOptions(
[property: CliArgument] string ServiceConnectionPolicy,
[property: CliOption("--network")] string Network,
[property: CliOption("--service-class")] string ServiceClass,
[property: CliOption("--subnets")] string[] Subnets,
[property: CliOption("--psc-connection-limit")] string PscConnectionLimit
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}
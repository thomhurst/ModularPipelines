using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "service-connection-policies", "list")]
public record GcloudNetworkConnectivityServiceConnectionPoliciesListOptions(
[property: CliOption("--region")] string Region
) : GcloudOptions;
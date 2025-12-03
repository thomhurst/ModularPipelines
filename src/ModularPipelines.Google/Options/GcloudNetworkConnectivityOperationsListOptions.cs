using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-connectivity", "operations", "list")]
public record GcloudNetworkConnectivityOperationsListOptions(
[property: CliOption("--region")] string Region
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-management", "connectivity-tests", "list")]
public record GcloudNetworkManagementConnectivityTestsListOptions : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-management", "connectivity-tests", "list")]
public record GcloudNetworkManagementConnectivityTestsListOptions : GcloudOptions;
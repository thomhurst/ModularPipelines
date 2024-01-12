using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-connectivity", "operations", "list")]
public record GcloudNetworkConnectivityOperationsListOptions(
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;
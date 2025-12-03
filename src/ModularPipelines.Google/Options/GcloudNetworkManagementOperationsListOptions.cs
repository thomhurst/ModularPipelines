using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-management", "operations", "list")]
public record GcloudNetworkManagementOperationsListOptions : GcloudOptions;
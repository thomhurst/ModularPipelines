using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "clusters", "delete")]
public record GcloudBigtableClustersDeleteOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Instance
) : GcloudOptions;
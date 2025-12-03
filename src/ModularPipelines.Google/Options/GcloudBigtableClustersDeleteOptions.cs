using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "clusters", "delete")]
public record GcloudBigtableClustersDeleteOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Instance
) : GcloudOptions;
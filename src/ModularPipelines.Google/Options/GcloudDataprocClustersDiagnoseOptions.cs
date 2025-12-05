using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataproc", "clusters", "diagnose")]
public record GcloudDataprocClustersDiagnoseOptions(
[property: CliArgument] string Cluster,
[property: CliArgument] string Region
) : GcloudOptions;
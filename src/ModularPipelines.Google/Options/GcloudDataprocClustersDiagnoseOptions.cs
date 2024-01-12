using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataproc", "clusters", "diagnose")]
public record GcloudDataprocClustersDiagnoseOptions(
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions;
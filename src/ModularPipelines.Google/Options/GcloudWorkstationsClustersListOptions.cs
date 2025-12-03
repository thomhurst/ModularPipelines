using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "clusters", "list")]
public record GcloudWorkstationsClustersListOptions(
[property: CliOption("--region")] string Region
) : GcloudOptions;
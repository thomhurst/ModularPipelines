using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workstations", "list-usable")]
public record GcloudWorkstationsListUsableOptions(
[property: CliOption("--config")] string Config,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--region")] string Region
) : GcloudOptions;
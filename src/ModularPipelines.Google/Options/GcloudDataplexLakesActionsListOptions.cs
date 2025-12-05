using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "lakes", "actions", "list")]
public record GcloudDataplexLakesActionsListOptions(
[property: CliOption("--lake")] string Lake,
[property: CliOption("--location")] string Location
) : GcloudOptions;
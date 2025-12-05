using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "assets", "list")]
public record GcloudDataplexAssetsListOptions(
[property: CliOption("--zone")] string Zone,
[property: CliOption("--lake")] string Lake,
[property: CliOption("--location")] string Location
) : GcloudOptions;
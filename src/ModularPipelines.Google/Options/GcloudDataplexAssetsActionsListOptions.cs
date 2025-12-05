using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "assets", "actions", "list")]
public record GcloudDataplexAssetsActionsListOptions(
[property: CliOption("--asset")] string Asset,
[property: CliOption("--lake")] string Lake,
[property: CliOption("--location")] string Location,
[property: CliOption("--zone")] string Zone
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "environments", "list")]
public record GcloudDataplexEnvironmentsListOptions(
[property: CliOption("--lake")] string Lake,
[property: CliOption("--location")] string Location
) : GcloudOptions;
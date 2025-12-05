using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "environments", "sessions", "list")]
public record GcloudDataplexEnvironmentsSessionsListOptions(
[property: CliOption("--environment")] string Environment,
[property: CliOption("--lake")] string Lake,
[property: CliOption("--location")] string Location
) : GcloudOptions;
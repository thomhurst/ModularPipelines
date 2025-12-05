using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "datascans", "run")]
public record GcloudDataplexDatascansRunOptions(
[property: CliArgument] string Datascan,
[property: CliArgument] string Location
) : GcloudOptions;
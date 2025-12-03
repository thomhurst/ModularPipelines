using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "lakes", "describe")]
public record GcloudDataplexLakesDescribeOptions(
[property: CliArgument] string Lake,
[property: CliArgument] string Location
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "assets", "describe")]
public record GcloudDataplexAssetsDescribeOptions(
[property: CliArgument] string Asset,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliArgument] string Zone
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "pools", "describe")]
public record GcloudPrivatecaPoolsDescribeOptions(
[property: CliArgument] string Pool,
[property: CliArgument] string Location
) : GcloudOptions;
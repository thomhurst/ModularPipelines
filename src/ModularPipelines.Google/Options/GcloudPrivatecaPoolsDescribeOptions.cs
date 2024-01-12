using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "pools", "describe")]
public record GcloudPrivatecaPoolsDescribeOptions(
[property: PositionalArgument] string Pool,
[property: PositionalArgument] string Location
) : GcloudOptions;
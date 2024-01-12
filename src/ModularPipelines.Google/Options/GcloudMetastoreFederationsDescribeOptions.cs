using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "federations", "describe")]
public record GcloudMetastoreFederationsDescribeOptions(
[property: PositionalArgument] string Federation,
[property: PositionalArgument] string Location
) : GcloudOptions;
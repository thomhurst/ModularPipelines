using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workstations", "configs", "describe")]
public record GcloudWorkstationsConfigsDescribeOptions(
[property: PositionalArgument] string Config,
[property: PositionalArgument] string Cluster,
[property: PositionalArgument] string Region
) : GcloudOptions;
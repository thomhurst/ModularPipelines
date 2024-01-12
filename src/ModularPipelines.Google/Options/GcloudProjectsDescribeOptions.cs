using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("projects", "describe")]
public record GcloudProjectsDescribeOptions(
[property: PositionalArgument] string ProjectIdOrNumber
) : GcloudOptions;
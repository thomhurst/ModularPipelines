using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments", "describe")]
public record GcloudComposerEnvironmentsDescribeOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Location
) : GcloudOptions;
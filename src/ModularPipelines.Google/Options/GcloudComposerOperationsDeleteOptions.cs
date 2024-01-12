using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "operations", "delete")]
public record GcloudComposerOperationsDeleteOptions(
[property: PositionalArgument] string Operations,
[property: PositionalArgument] string Location
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "custom-jobs", "cancel")]
public record GcloudAiCustomJobsCancelOptions(
[property: PositionalArgument] string CustomJob,
[property: PositionalArgument] string Region
) : GcloudOptions;
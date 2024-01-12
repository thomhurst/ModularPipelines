using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "models", "delete")]
public record GcloudAiModelsDeleteOptions(
[property: PositionalArgument] string Model,
[property: PositionalArgument] string Region
) : GcloudOptions;
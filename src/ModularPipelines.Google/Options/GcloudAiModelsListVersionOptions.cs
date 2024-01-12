using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "models", "list-version")]
public record GcloudAiModelsListVersionOptions(
[property: PositionalArgument] string Model,
[property: PositionalArgument] string Region
) : GcloudOptions;
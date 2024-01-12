using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "models", "delete-version")]
public record GcloudAiModelsDeleteVersionOptions(
[property: PositionalArgument] string ModelVersion,
[property: PositionalArgument] string Region
) : GcloudOptions;
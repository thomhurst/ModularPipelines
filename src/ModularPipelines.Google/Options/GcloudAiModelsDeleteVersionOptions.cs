using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "models", "delete-version")]
public record GcloudAiModelsDeleteVersionOptions(
[property: CliArgument] string ModelVersion,
[property: CliArgument] string Region
) : GcloudOptions;
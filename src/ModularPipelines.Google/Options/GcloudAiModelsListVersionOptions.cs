using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "models", "list-version")]
public record GcloudAiModelsListVersionOptions(
[property: CliArgument] string Model,
[property: CliArgument] string Region
) : GcloudOptions;
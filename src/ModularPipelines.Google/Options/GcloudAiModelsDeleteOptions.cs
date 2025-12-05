using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "models", "delete")]
public record GcloudAiModelsDeleteOptions(
[property: CliArgument] string Model,
[property: CliArgument] string Region
) : GcloudOptions;
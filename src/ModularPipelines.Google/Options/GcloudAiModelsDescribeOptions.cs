using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "models", "describe")]
public record GcloudAiModelsDescribeOptions(
[property: CliArgument] string Model,
[property: CliArgument] string Region
) : GcloudOptions;
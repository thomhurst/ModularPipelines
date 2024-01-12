using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "models", "describe")]
public record GcloudAiModelsDescribeOptions(
[property: PositionalArgument] string Model,
[property: PositionalArgument] string Region
) : GcloudOptions;
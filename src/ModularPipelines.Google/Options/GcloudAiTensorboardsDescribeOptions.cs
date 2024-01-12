using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "tensorboards", "describe")]
public record GcloudAiTensorboardsDescribeOptions(
[property: PositionalArgument] string Tensorboard,
[property: PositionalArgument] string Region
) : GcloudOptions;
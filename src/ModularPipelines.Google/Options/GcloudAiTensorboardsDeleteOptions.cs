using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "tensorboards", "delete")]
public record GcloudAiTensorboardsDeleteOptions(
[property: PositionalArgument] string Tensorboard,
[property: PositionalArgument] string Region
) : GcloudOptions;
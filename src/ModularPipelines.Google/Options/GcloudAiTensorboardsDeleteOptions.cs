using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "tensorboards", "delete")]
public record GcloudAiTensorboardsDeleteOptions(
[property: CliArgument] string Tensorboard,
[property: CliArgument] string Region
) : GcloudOptions;
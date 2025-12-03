using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "tensorboards", "describe")]
public record GcloudAiTensorboardsDescribeOptions(
[property: CliArgument] string Tensorboard,
[property: CliArgument] string Region
) : GcloudOptions;
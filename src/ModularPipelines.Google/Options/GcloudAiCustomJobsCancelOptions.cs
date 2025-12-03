using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "custom-jobs", "cancel")]
public record GcloudAiCustomJobsCancelOptions(
[property: CliArgument] string CustomJob,
[property: CliArgument] string Region
) : GcloudOptions;
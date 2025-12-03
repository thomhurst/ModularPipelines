using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("config", "get")]
public record GcloudConfigGetOptions(
[property: CliArgument] string Section
) : GcloudOptions;
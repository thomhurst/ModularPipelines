using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("projects", "delete")]
public record GcloudProjectsDeleteOptions(
[property: CliArgument] string ProjectIdOrNumber
) : GcloudOptions;
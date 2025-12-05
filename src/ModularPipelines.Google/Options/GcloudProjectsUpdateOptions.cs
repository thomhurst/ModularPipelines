using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("projects", "update")]
public record GcloudProjectsUpdateOptions(
[property: CliArgument] string ProjectId,
[property: CliOption("--name")] string Name
) : GcloudOptions;
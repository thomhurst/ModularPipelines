using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("projects", "get-ancestors")]
public record GcloudProjectsGetAncestorsOptions(
[property: CliArgument] string ProjectId
) : GcloudOptions;
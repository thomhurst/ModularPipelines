using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("projects", "get-ancestors")]
public record GcloudProjectsGetAncestorsOptions(
[property: PositionalArgument] string ProjectId
) : GcloudOptions;
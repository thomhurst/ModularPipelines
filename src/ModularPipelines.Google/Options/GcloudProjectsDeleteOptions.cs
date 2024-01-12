using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("projects", "delete")]
public record GcloudProjectsDeleteOptions(
[property: PositionalArgument] string ProjectIdOrNumber
) : GcloudOptions;
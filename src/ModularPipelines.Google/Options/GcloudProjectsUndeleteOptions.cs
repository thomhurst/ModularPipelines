using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("projects", "undelete")]
public record GcloudProjectsUndeleteOptions(
[property: PositionalArgument] string ProjectIdOrNumber
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("projects", "undelete")]
public record GcloudProjectsUndeleteOptions(
[property: CliArgument] string ProjectIdOrNumber
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "projects", "unlink")]
public record GcloudBillingProjectsUnlinkOptions(
[property: PositionalArgument] string ProjectId
) : GcloudOptions;
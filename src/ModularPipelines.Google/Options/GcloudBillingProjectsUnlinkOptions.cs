using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "projects", "unlink")]
public record GcloudBillingProjectsUnlinkOptions(
[property: CliArgument] string ProjectId
) : GcloudOptions;
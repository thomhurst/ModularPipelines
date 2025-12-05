using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-approval", "requests", "dismiss")]
public record GcloudAccessApprovalRequestsDismissOptions(
[property: CliArgument] string Name
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-approval", "requests", "approve")]
public record GcloudAccessApprovalRequestsApproveOptions(
[property: CliArgument] string Name
) : GcloudOptions;
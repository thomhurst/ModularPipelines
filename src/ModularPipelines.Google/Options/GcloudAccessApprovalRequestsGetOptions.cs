using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-approval", "requests", "get")]
public record GcloudAccessApprovalRequestsGetOptions(
[property: CliArgument] string Name
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-approval", "requests", "invalidate")]
public record GcloudAccessApprovalRequestsInvalidateOptions(
[property: CliArgument] string Name
) : GcloudOptions;
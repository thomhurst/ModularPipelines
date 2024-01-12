using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-approval", "requests", "approve")]
public record GcloudAccessApprovalRequestsApproveOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-approval", "requests", "get")]
public record GcloudAccessApprovalRequestsGetOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;
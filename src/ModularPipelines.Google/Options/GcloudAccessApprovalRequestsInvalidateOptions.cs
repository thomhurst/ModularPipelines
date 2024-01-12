using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-approval", "requests", "invalidate")]
public record GcloudAccessApprovalRequestsInvalidateOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;
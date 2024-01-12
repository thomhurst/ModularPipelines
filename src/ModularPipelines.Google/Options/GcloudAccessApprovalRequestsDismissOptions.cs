using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-approval", "requests", "dismiss")]
public record GcloudAccessApprovalRequestsDismissOptions(
[property: PositionalArgument] string Name
) : GcloudOptions;
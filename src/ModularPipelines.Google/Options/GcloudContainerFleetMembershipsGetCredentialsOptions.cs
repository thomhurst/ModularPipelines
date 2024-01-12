using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "memberships", "get-credentials")]
public record GcloudContainerFleetMembershipsGetCredentialsOptions(
[property: PositionalArgument] string MembershipName,
[property: PositionalArgument] string Location
) : GcloudOptions;
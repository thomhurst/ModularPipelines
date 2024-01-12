using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "memberships", "get-credentials")]
public record GcloudContainerHubMembershipsGetCredentialsOptions(
[property: PositionalArgument] string MembershipName,
[property: PositionalArgument] string Location
) : GcloudOptions;
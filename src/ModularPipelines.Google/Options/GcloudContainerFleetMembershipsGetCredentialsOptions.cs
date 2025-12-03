using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "memberships", "get-credentials")]
public record GcloudContainerFleetMembershipsGetCredentialsOptions(
[property: CliArgument] string MembershipName,
[property: CliArgument] string Location
) : GcloudOptions;
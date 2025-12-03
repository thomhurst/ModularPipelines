using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "memberships", "get-credentials")]
public record GcloudContainerHubMembershipsGetCredentialsOptions(
[property: CliArgument] string MembershipName,
[property: CliArgument] string Location
) : GcloudOptions;
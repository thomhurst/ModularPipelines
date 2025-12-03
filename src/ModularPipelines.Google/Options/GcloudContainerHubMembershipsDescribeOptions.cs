using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "memberships", "describe")]
public record GcloudContainerHubMembershipsDescribeOptions(
[property: CliArgument] string Membership,
[property: CliArgument] string Location
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "memberships", "describe")]
public record GcloudContainerFleetMembershipsDescribeOptions(
[property: CliArgument] string Membership,
[property: CliArgument] string Location
) : GcloudOptions;
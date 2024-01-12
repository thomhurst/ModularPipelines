using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "memberships", "describe")]
public record GcloudContainerHubMembershipsDescribeOptions(
[property: PositionalArgument] string Membership,
[property: PositionalArgument] string Location
) : GcloudOptions;
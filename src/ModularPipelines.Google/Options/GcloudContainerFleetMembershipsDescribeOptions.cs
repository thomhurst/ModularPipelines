using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "memberships", "describe")]
public record GcloudContainerFleetMembershipsDescribeOptions(
[property: PositionalArgument] string Membership,
[property: PositionalArgument] string Location
) : GcloudOptions;
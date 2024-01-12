using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "fleet", "memberships", "bindings", "describe")]
public record GcloudContainerFleetMembershipsBindingsDescribeOptions(
[property: PositionalArgument] string Binding,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Membership
) : GcloudOptions;
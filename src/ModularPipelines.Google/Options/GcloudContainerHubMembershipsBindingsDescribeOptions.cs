using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "memberships", "bindings", "describe")]
public record GcloudContainerHubMembershipsBindingsDescribeOptions(
[property: PositionalArgument] string Binding,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Membership
) : GcloudOptions;
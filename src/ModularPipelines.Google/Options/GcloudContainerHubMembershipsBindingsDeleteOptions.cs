using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "hub", "memberships", "bindings", "delete")]
public record GcloudContainerHubMembershipsBindingsDeleteOptions(
[property: PositionalArgument] string Binding,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Membership
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "fleet", "memberships", "bindings", "describe")]
public record GcloudContainerFleetMembershipsBindingsDescribeOptions(
[property: CliArgument] string Binding,
[property: CliArgument] string Location,
[property: CliArgument] string Membership
) : GcloudOptions;
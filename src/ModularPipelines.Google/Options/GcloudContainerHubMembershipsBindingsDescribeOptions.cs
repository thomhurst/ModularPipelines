using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "memberships", "bindings", "describe")]
public record GcloudContainerHubMembershipsBindingsDescribeOptions(
[property: CliArgument] string Binding,
[property: CliArgument] string Location,
[property: CliArgument] string Membership
) : GcloudOptions;
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "memberships", "bindings", "delete")]
public record GcloudContainerHubMembershipsBindingsDeleteOptions(
[property: CliArgument] string Binding,
[property: CliArgument] string Location,
[property: CliArgument] string Membership
) : GcloudOptions;
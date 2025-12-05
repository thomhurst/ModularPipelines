using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "memberships", "bindings", "create")]
public record GcloudContainerHubMembershipsBindingsCreateOptions(
[property: CliArgument] string Binding,
[property: CliArgument] string Location,
[property: CliArgument] string Membership,
[property: CliOption("--scope")] string Scope
) : GcloudOptions
{
    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "memberships", "bindings", "update")]
public record GcloudContainerHubMembershipsBindingsUpdateOptions(
[property: CliArgument] string Binding,
[property: CliArgument] string Location,
[property: CliArgument] string Membership,
[property: CliOption("--scope")] string Scope
) : GcloudOptions
{
    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}
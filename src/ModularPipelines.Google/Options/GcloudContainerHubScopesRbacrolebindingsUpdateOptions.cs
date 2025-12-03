using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "scopes", "rbacrolebindings", "update")]
public record GcloudContainerHubScopesRbacrolebindingsUpdateOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Location,
[property: CliArgument] string Scope
) : GcloudOptions
{
    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--group")]
    public string? Group { get; set; }

    [CliOption("--user")]
    public string? User { get; set; }
}
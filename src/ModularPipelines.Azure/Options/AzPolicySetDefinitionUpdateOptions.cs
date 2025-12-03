using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("policy", "set-definition", "update")]
public record AzPolicySetDefinitionUpdateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--definition-groups")]
    public string? DefinitionGroups { get; set; }

    [CliOption("--definitions")]
    public string? Definitions { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--params")]
    public string? Params { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
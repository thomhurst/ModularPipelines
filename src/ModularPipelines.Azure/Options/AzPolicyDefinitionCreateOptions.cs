using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("policy", "definition", "create")]
public record AzPolicyDefinitionCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--management-group")]
    public string? ManagementGroup { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--params")]
    public string? Params { get; set; }

    [CliOption("--rules")]
    public string? Rules { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
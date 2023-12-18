using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "exemption", "create")]
public record AzPolicyExemptionCreateOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--exemption-category")]
    public string? ExemptionCategory { get; set; }

    [CommandSwitch("--expires-on")]
    public string? ExpiresOn { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--policy-assignment")]
    public string? PolicyAssignment { get; set; }

    [CommandSwitch("--policy-definition-reference-ids")]
    public string? PolicyDefinitionReferenceIds { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}
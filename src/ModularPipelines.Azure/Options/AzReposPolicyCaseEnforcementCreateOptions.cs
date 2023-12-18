using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "policy", "case-enforcement", "create")]
public record AzReposPolicyCaseEnforcementCreateOptions(
[property: BooleanCommandSwitch("--blocking")] bool Blocking,
[property: BooleanCommandSwitch("--enabled")] bool Enabled,
[property: CommandSwitch("--repository-id")] string RepositoryId
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}
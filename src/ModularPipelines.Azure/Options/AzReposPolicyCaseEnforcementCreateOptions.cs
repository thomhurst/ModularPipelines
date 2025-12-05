using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "policy", "case-enforcement", "create")]
public record AzReposPolicyCaseEnforcementCreateOptions(
[property: CliFlag("--blocking")] bool Blocking,
[property: CliFlag("--enabled")] bool Enabled,
[property: CliOption("--repository-id")] string RepositoryId
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}
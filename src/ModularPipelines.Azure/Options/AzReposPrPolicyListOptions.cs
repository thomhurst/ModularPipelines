using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("repos", "pr", "policy", "list")]
public record AzReposPrPolicyListOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--skip")]
    public string? Skip { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}
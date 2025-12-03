using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops", "security", "group", "delete")]
public record AzDevopsSecurityGroupDeleteOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}
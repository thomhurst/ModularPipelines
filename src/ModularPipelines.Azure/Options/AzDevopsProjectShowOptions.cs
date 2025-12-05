using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "project", "show")]
public record AzDevopsProjectShowOptions(
[property: CliOption("--project")] string Project
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--open")]
    public bool? Open { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}
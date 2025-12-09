using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "project", "create")]
public record AzDevopsProjectCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--open")]
    public bool? Open { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--process")]
    public string? Process { get; set; }

    [CliOption("--source-control")]
    public string? SourceControl { get; set; }

    [CliOption("--visibility")]
    public string? Visibility { get; set; }
}
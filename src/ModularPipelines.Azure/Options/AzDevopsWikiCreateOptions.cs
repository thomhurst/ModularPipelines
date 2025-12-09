using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "wiki", "create")]
public record AzDevopsWikiCreateOptions : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--mapped-path")]
    public string? MappedPath { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}
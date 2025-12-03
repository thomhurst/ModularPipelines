using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops", "extension", "list")]
public record AzDevopsExtensionListOptions : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--include-built-in")]
    public bool? IncludeBuiltIn { get; set; }

    [CliFlag("--include-disabled")]
    public bool? IncludeDisabled { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}
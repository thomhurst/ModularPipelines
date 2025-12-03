using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "connection", "list")]
public record AzSpringConnectionListOptions : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--source-id")]
    public string? SourceId { get; set; }
}
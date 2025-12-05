using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
public record AzOptions() : CommandLineToolOptions("az")
{
    [CliFlag("--debug")]
    public bool? Debug { get; set; }

    [CliFlag("--only-show-errors")]
    public bool? OnlyShowErrors { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliOption("--query")]
    public string? Query { get; set; }

    [CliOption("--subscription")]
    public string? Subscription { get; set; }

    [CliFlag("--verbose")]
    public bool? Verbose { get; set; }
}

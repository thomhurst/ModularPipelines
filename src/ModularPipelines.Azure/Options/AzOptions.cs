using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
public record AzOptions() : CommandLineToolOptions("az")
{
    [BooleanCommandSwitch("--debug")]
    public bool? Debug { get; set; }

    [BooleanCommandSwitch("--only-show-errors")]
    public bool? OnlyShowErrors { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--query")]
    public string? Query { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--verbose")]
    public bool? Verbose { get; set; }
}

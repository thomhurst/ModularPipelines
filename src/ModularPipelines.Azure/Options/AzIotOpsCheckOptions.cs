using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "ops", "check")]
public record AzIotOpsCheckOptions : AzOptions
{
    [CliFlag("--as-object")]
    public bool? AsObject { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--detail-level")]
    public string? DetailLevel { get; set; }

    [CliOption("--ops-service")]
    public string? OpsService { get; set; }

    [CliFlag("--post")]
    public bool? Post { get; set; }

    [CliFlag("--pre")]
    public bool? Pre { get; set; }

    [CliOption("--resources")]
    public string? Resources { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("footprint", "profile", "create")]
public record AzFootprintProfileCreateOptions(
[property: CommandSwitch("--measurement-count")] int MeasurementCount,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--start-delay-ms")] string StartDelayMs
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--reporting-endpoints")]
    public string? ReportingEndpoints { get; set; }

    [CommandSwitch("--sample-rate-cold")]
    public string? SampleRateCold { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}
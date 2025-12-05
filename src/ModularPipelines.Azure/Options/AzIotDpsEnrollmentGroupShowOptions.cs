using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "dps", "enrollment-group", "show")]
public record AzIotDpsEnrollmentGroupShowOptions(
[property: CliOption("--eid")] string Eid
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--dps-name")]
    public string? DpsName { get; set; }

    [CliFlag("--keys")]
    public bool? Keys { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
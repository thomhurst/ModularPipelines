using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "dps", "enrollment", "list")]
public record AzIotDpsEnrollmentListOptions : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--dps-name")]
    public string? DpsName { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}
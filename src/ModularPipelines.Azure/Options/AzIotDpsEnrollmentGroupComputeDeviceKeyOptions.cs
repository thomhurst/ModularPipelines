using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "dps", "enrollment-group", "compute-device-key")]
public record AzIotDpsEnrollmentGroupComputeDeviceKeyOptions(
[property: CliOption("--registration-id")] string RegistrationId
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--dps-name")]
    public string? DpsName { get; set; }

    [CliOption("--eid")]
    public string? Eid { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
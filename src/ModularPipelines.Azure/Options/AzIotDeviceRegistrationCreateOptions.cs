using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "device", "registration", "create")]
public record AzIotDeviceRegistrationCreateOptions(
[property: CommandSwitch("--registration-id")] string RegistrationId
) : AzOptions
{
    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--certificate-file-path")]
    public string? CertificateFilePath { get; set; }

    [BooleanCommandSwitch("--ck")]
    public bool? Ck { get; set; }

    [CommandSwitch("--dps-name")]
    public string? DpsName { get; set; }

    [CommandSwitch("--enrollment-group-id")]
    public string? EnrollmentGroupId { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--id-scope")]
    public string? IdScope { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--key-file-path")]
    public string? KeyFilePath { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--pass")]
    public string? Pass { get; set; }

    [CommandSwitch("--payload")]
    public string? Payload { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}


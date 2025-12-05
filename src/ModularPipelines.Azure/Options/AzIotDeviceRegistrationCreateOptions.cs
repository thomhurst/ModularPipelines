using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "device", "registration", "create")]
public record AzIotDeviceRegistrationCreateOptions(
[property: CliOption("--registration-id")] string RegistrationId
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--certificate-file-path")]
    public string? CertificateFilePath { get; set; }

    [CliFlag("--ck")]
    public bool? Ck { get; set; }

    [CliOption("--dps-name")]
    public string? DpsName { get; set; }

    [CliOption("--enrollment-group-id")]
    public string? EnrollmentGroupId { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--id-scope")]
    public string? IdScope { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--key-file-path")]
    public string? KeyFilePath { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--pass")]
    public string? Pass { get; set; }

    [CliOption("--payload")]
    public string? Payload { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
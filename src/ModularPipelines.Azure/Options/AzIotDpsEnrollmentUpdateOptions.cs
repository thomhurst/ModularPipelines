using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "dps", "enrollment", "update")]
public record AzIotDpsEnrollmentUpdateOptions(
[property: CliOption("--eid")] string Eid
) : AzOptions
{
    [CliOption("--allocation-policy")]
    public string? AllocationPolicy { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CliOption("--device-id")]
    public string? DeviceId { get; set; }

    [CliOption("--device-information")]
    public string? DeviceInformation { get; set; }

    [CliOption("--dps-name")]
    public string? DpsName { get; set; }

    [CliFlag("--edge-enabled")]
    public bool? EdgeEnabled { get; set; }

    [CliOption("--ek")]
    public string? Ek { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--ih")]
    public string? Ih { get; set; }

    [CliOption("--initial-twin-properties")]
    public string? InitialTwinProperties { get; set; }

    [CliOption("--initial-twin-tags")]
    public string? InitialTwinTags { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--pk")]
    public string? Pk { get; set; }

    [CliOption("--provisioning-status")]
    public string? ProvisioningStatus { get; set; }

    [CliFlag("--rc")]
    public bool? Rc { get; set; }

    [CliFlag("--remove-secondary-certificate")]
    public bool? RemoveSecondaryCertificate { get; set; }

    [CliOption("--reprovision-policy")]
    public string? ReprovisionPolicy { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scp")]
    public string? Scp { get; set; }

    [CliOption("--secondary-key")]
    public string? SecondaryKey { get; set; }

    [CliOption("--webhook-url")]
    public string? WebhookUrl { get; set; }
}
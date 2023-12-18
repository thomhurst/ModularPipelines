using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps", "enrollment-group", "update")]
public record AzIotDpsEnrollmentGroupUpdateOptions(
[property: CommandSwitch("--eid")] string Eid
) : AzOptions
{
    [CommandSwitch("--allocation-policy")]
    public string? AllocationPolicy { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--auth-type")]
    public string? AuthType { get; set; }

    [CommandSwitch("--ca-name")]
    public string? CaName { get; set; }

    [CommandSwitch("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CommandSwitch("--dps-name")]
    public string? DpsName { get; set; }

    [BooleanCommandSwitch("--edge-enabled")]
    public bool? EdgeEnabled { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--ih")]
    public string? Ih { get; set; }

    [CommandSwitch("--initial-twin-properties")]
    public string? InitialTwinProperties { get; set; }

    [CommandSwitch("--initial-twin-tags")]
    public string? InitialTwinTags { get; set; }

    [CommandSwitch("--login")]
    public string? Login { get; set; }

    [CommandSwitch("--pk")]
    public string? Pk { get; set; }

    [CommandSwitch("--provisioning-status")]
    public string? ProvisioningStatus { get; set; }

    [BooleanCommandSwitch("--rc")]
    public bool? Rc { get; set; }

    [BooleanCommandSwitch("--remove-secondary-certificate")]
    public bool? RemoveSecondaryCertificate { get; set; }

    [CommandSwitch("--reprovision-policy")]
    public string? ReprovisionPolicy { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scn")]
    public string? Scn { get; set; }

    [CommandSwitch("--scp")]
    public string? Scp { get; set; }

    [CommandSwitch("--secondary-key")]
    public string? SecondaryKey { get; set; }

    [CommandSwitch("--webhook-url")]
    public string? WebhookUrl { get; set; }
}
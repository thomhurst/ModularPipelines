using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "enrollment-group", "update")]
public record AzIotCentralEnrollmentGroupUpdateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--group-id")] string GroupId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--certificate-entry")]
    public string? CertificateEntry { get; set; }

    [CommandSwitch("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--provisioning-status")]
    public string? ProvisioningStatus { get; set; }

    [BooleanCommandSwitch("--remove-x509")]
    public bool? RemoveX509 { get; set; }

    [CommandSwitch("--scp")]
    public string? Scp { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}
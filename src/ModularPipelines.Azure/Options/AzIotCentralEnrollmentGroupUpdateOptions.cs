using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "enrollment-group", "update")]
public record AzIotCentralEnrollmentGroupUpdateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--group-id")] string GroupId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--certificate-entry")]
    public string? CertificateEntry { get; set; }

    [CliOption("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--provisioning-status")]
    public string? ProvisioningStatus { get; set; }

    [CliFlag("--remove-x509")]
    public bool? RemoveX509 { get; set; }

    [CliOption("--scp")]
    public string? Scp { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}
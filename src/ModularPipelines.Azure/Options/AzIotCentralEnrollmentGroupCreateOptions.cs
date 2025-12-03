using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "enrollment-group", "create")]
public record AzIotCentralEnrollmentGroupCreateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--at")] string At,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--pk")]
    public string? Pk { get; set; }

    [CliOption("--provisioning-status")]
    public string? ProvisioningStatus { get; set; }

    [CliOption("--scp")]
    public string? Scp { get; set; }

    [CliOption("--secondary-key")]
    public string? SecondaryKey { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}
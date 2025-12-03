using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "enrollment-group", "verify-certificate")]
public record AzIotCentralEnrollmentGroupVerifyCertificateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--group-id")] string GroupId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CliOption("--scp")]
    public string? Scp { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "enrollment-group", "generate-verification-code")]
public record AzIotCentralEnrollmentGroupGenerateVerificationCodeOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--group-id")] string GroupId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--certificate-entry")]
    public string? CertificateEntry { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}
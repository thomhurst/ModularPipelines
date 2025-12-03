using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "ekm-connections", "create")]
public record GcloudKmsEkmConnectionsCreateOptions(
[property: CliArgument] string EkmConnection,
[property: CliArgument] string Location,
[property: CliOption("--hostname")] string Hostname,
[property: CliOption("--server-certificates-files")] string[] ServerCertificatesFiles,
[property: CliOption("--service-directory-service")] string ServiceDirectoryService
) : GcloudOptions
{
    [CliOption("--endpoint-filter")]
    public string? EndpointFilter { get; set; }

    [CliOption("--crypto-space-path")]
    public string? CryptoSpacePath { get; set; }

    [CliOption("--key-management-mode")]
    public string? KeyManagementMode { get; set; }
}
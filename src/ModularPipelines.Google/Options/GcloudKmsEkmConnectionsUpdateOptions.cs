using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "ekm-connections", "update")]
public record GcloudKmsEkmConnectionsUpdateOptions(
[property: CliArgument] string EkmConnection,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--endpoint-filter")]
    public string? EndpointFilter { get; set; }

    [CliOption("--hostname")]
    public string? Hostname { get; set; }

    [CliOption("--server-certificates-files")]
    public string[]? ServerCertificatesFiles { get; set; }

    [CliOption("--service-directory-service")]
    public string? ServiceDirectoryService { get; set; }

    [CliOption("--crypto-space-path")]
    public string? CryptoSpacePath { get; set; }

    [CliOption("--key-management-mode")]
    public string? KeyManagementMode { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "ekm-connections", "update")]
public record GcloudKmsEkmConnectionsUpdateOptions(
[property: PositionalArgument] string EkmConnection,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--endpoint-filter")]
    public string? EndpointFilter { get; set; }

    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--server-certificates-files")]
    public string[]? ServerCertificatesFiles { get; set; }

    [CommandSwitch("--service-directory-service")]
    public string? ServiceDirectoryService { get; set; }

    [CommandSwitch("--crypto-space-path")]
    public string? CryptoSpacePath { get; set; }

    [CommandSwitch("--key-management-mode")]
    public string? KeyManagementMode { get; set; }
}
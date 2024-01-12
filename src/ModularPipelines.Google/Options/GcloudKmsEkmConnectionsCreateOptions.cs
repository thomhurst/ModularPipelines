using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "ekm-connections", "create")]
public record GcloudKmsEkmConnectionsCreateOptions(
[property: PositionalArgument] string EkmConnection,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--hostname")] string Hostname,
[property: CommandSwitch("--server-certificates-files")] string[] ServerCertificatesFiles,
[property: CommandSwitch("--service-directory-service")] string ServiceDirectoryService
) : GcloudOptions
{
    [CommandSwitch("--endpoint-filter")]
    public string? EndpointFilter { get; set; }

    [CommandSwitch("--crypto-space-path")]
    public string? CryptoSpacePath { get; set; }

    [CommandSwitch("--key-management-mode")]
    public string? KeyManagementMode { get; set; }
}
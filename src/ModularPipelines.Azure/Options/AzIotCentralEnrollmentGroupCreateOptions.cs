using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "enrollment-group", "create")]
public record AzIotCentralEnrollmentGroupCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--at")] string At,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--pk")]
    public string? Pk { get; set; }

    [CommandSwitch("--provisioning-status")]
    public string? ProvisioningStatus { get; set; }

    [CommandSwitch("--scp")]
    public string? Scp { get; set; }

    [CommandSwitch("--secondary-key")]
    public string? SecondaryKey { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}
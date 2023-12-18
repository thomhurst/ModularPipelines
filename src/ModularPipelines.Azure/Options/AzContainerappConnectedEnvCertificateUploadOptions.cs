using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connected-env", "certificate", "upload")]
public record AzContainerappConnectedEnvCertificateUploadOptions(
[property: CommandSwitch("--certificate-file")] string CertificateFile
) : AzOptions
{
    [CommandSwitch("--certificate-name")]
    public string? CertificateName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--show-prompt")]
    public bool? ShowPrompt { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps", "certificate", "delete")]
public record AzIotDpsCertificateDeleteOptions(
[property: CommandSwitch("--certificate-name")] string CertificateName,
[property: CommandSwitch("--dps-name")] string DpsName,
[property: CommandSwitch("--etag")] string Etag
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}
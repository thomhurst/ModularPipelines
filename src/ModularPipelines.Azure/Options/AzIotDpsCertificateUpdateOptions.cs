using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "dps", "certificate", "update")]
public record AzIotDpsCertificateUpdateOptions(
[property: CommandSwitch("--certificate-name")] string CertificateName,
[property: CommandSwitch("--dps-name")] string DpsName,
[property: CommandSwitch("--etag")] string Etag,
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--verified")]
    public bool? Verified { get; set; }
}
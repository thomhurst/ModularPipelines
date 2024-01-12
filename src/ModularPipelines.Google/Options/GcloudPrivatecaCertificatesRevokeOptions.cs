using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "certificates", "revoke")]
public record GcloudPrivatecaCertificatesRevokeOptions(
[property: CommandSwitch("--certificate")] string Certificate,
[property: CommandSwitch("--serial-number")] string SerialNumber
) : GcloudOptions
{
    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--issuer-pool")]
    public string? IssuerPool { get; set; }

    [CommandSwitch("--issuer-location")]
    public string? IssuerLocation { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "certificates", "list")]
public record GcloudPrivatecaCertificatesListOptions : GcloudOptions
{
    [CommandSwitch("--issuer-pool")]
    public string? IssuerPool { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}
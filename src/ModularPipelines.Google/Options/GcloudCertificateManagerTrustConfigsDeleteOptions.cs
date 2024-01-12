using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "trust-configs", "delete")]
public record GcloudCertificateManagerTrustConfigsDeleteOptions(
[property: PositionalArgument] string TrustConfig,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}
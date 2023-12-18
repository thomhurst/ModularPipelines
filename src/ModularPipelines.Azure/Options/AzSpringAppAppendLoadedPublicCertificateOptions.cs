using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "app", "append-loaded-public-certificate")]
public record AzSpringAppAppendLoadedPublicCertificateOptions(
[property: CommandSwitch("--certificate-name")] string CertificateName,
[property: BooleanCommandSwitch("--load-trust-store")] bool LoadTrustStore,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [BooleanCommandSwitch("--enable-sub-path")]
    public bool? EnableSubPath { get; set; }

    [CommandSwitch("--mount-options")]
    public string? MountOptions { get; set; }

    [BooleanCommandSwitch("--read-only")]
    public bool? ReadOnly { get; set; }

    [CommandSwitch("--share-name")]
    public string? ShareName { get; set; }
}


using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("nuget", "sign", "[<package-path(s)>]")]
[ExcludeFromCodeCoverage]
public record DotNetNugetSignOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "[<package-path(s)>]")]
    public string? PackagePath { get; set; }

    [CommandSwitch("--certificate-path")]
    public string? CertificatePath { get; set; }

    [CommandSwitch("--certificate-store-name")]
    public string? CertificateStoreName { get; set; }

    [CommandSwitch("--certificate-store-location")]
    public string? CertificateStoreLocation { get; set; }

    [CommandSwitch("--certificate-subject-name")]
    public string? CertificateSubjectName { get; set; }

    [CommandSwitch("--certificate-fingerprint")]
    public string? CertificateFingerprint { get; set; }

    [CommandSwitch("--certificate-password")]
    public string? CertificatePassword { get; set; }

    [CommandSwitch("--hash-algorithm")]
    public string? Hashalgorithm { get; set; }

    [CommandSwitch("--output")]
    public string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }

    [CommandSwitch("--timestamp-hash-algorithm")]
    public string? TimestampHashAlgorithm { get; set; }

    [CommandSwitch("--timestamper")]
    public string? Timestamper { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }
}

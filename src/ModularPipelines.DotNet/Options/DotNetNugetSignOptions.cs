using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetSignOptions : DotNetOptions
{
    public DotNetNugetSignOptions(
        string packagePath
    )
    {
        CommandParts = ["nuget", "sign", "[<package-path(s)>]"];

        PackagePath = packagePath;
    }

    public DotNetNugetSignOptions()
    {
        CommandParts = ["nuget", "sign", "[<package-path(s)>]"];
    }

    [PositionalArgument(PlaceholderName = "[<package-path(s)>]")]
    public string? PackagePath { get; set; }

    [CommandSwitch("--certificate-path")]
    public virtual string? CertificatePath { get; set; }

    [CommandSwitch("--certificate-store-name")]
    public virtual string? CertificateStoreName { get; set; }

    [CommandSwitch("--certificate-store-location")]
    public virtual string? CertificateStoreLocation { get; set; }

    [CommandSwitch("--certificate-subject-name")]
    public virtual string? CertificateSubjectName { get; set; }

    [CommandSwitch("--certificate-fingerprint")]
    public virtual string? CertificateFingerprint { get; set; }

    [CommandSwitch("--certificate-password")]
    public virtual string? CertificatePassword { get; set; }

    [CommandSwitch("--hash-algorithm")]
    public virtual string? Hashalgorithm { get; set; }

    [CommandSwitch("--output")]
    public virtual string? OutputDirectory { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public virtual bool? Overwrite { get; set; }

    [CommandSwitch("--timestamp-hash-algorithm")]
    public virtual string? TimestampHashAlgorithm { get; set; }

    [CommandSwitch("--timestamper")]
    public virtual string? Timestamper { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }
}

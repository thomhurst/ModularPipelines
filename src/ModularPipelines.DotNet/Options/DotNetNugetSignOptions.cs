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

    [CliArgument(Name = "[<package-path(s)>]")]
    public string? PackagePath { get; set; }

    [CliOption("--certificate-path")]
    public virtual string? CertificatePath { get; set; }

    [CliOption("--certificate-store-name")]
    public virtual string? CertificateStoreName { get; set; }

    [CliOption("--certificate-store-location")]
    public virtual string? CertificateStoreLocation { get; set; }

    [CliOption("--certificate-subject-name")]
    public virtual string? CertificateSubjectName { get; set; }

    [CliOption("--certificate-fingerprint")]
    public virtual string? CertificateFingerprint { get; set; }

    [CliOption("--certificate-password")]
    public virtual string? CertificatePassword { get; set; }

    [CliOption("--hash-algorithm")]
    public virtual string? Hashalgorithm { get; set; }

    [CliOption("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CliFlag("--overwrite")]
    public virtual bool? Overwrite { get; set; }

    [CliOption("--timestamp-hash-algorithm")]
    public virtual string? TimestampHashAlgorithm { get; set; }

    [CliOption("--timestamper")]
    public virtual string? Timestamper { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }
}

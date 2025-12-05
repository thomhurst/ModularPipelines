using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetVerifyOptions : DotNetOptions
{
    public DotNetNugetVerifyOptions(
        string packagePath
    )
    {
        CommandParts = ["nuget", "verify", "[<package-path(s)>]"];

        PackagePath = packagePath;
    }

    public DotNetNugetVerifyOptions()
    {
        CommandParts = ["nuget", "verify", "[<package-path(s)>]"];
    }

    [CliArgument(Name = "[<package-path(s)>]")]
    public virtual string? PackagePath { get; set; }

    [CliFlag("--all")]
    public virtual bool? All { get; set; }

    [CliOption("--certificate-fingerprint")]
    public virtual IEnumerable<string>? CertificateFingerprint { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CliOption("--configfile")]
    public virtual string? Configfile { get; set; }
}

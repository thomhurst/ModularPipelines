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

    [PositionalArgument(PlaceholderName = "[<package-path(s)>]")]
    public string? PackagePath { get; set; }

    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--certificate-fingerprint")]
    public IEnumerable<string>? CertificateFingerprint { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }

    [CommandSwitch("--configfile")]
    public string? Configfile { get; set; }
}

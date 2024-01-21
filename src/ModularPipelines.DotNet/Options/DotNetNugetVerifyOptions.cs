using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("nuget", "verify", "[<package-path(s)>]")]
[ExcludeFromCodeCoverage]
public record DotNetNugetVerifyOptions : DotNetOptions
{
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

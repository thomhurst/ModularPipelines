using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetToolInstallOptions : DotNetOptions
{
    public DotNetToolInstallOptions(
        string packageName,
        string path
    )
    {
        CommandParts = ["tool", "install", "<PACKAGE_NAME>"];

        PackageName = packageName;

        ToolPath = path;
    }

    public DotNetToolInstallOptions(
        string packageName
    )
    {
        CommandParts = ["tool", "install", "<PACKAGE_NAME>"];

        PackageName = packageName;
    }

    [PositionalArgument(PlaceholderName = "<PACKAGE_NAME>")]
    public string? PackageName { get; set; }

    [BooleanCommandSwitch("--global")]
    public virtual bool? Global { get; set; }

    [CommandSwitch("--arch")]
    public virtual string? Architecture { get; set; }

    [CommandSwitch("--add-source")]
    public virtual string? AddSource { get; set; }

    [CommandSwitch("--configfile")]
    public virtual string? Configfile { get; set; }

    [BooleanCommandSwitch("--disable-parallel")]
    public virtual bool? DisableParallel { get; set; }

    [CommandSwitch("--framework")]
    public virtual string? Framework { get; set; }

    [BooleanCommandSwitch("--ignore-failed-sources")]
    public virtual bool? IgnoreFailedSources { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CommandSwitch("--tool-manifest")]
    public virtual string? ToolManifest { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CommandSwitch("--version")]
    public virtual string? VersionNumber { get; set; }

    [BooleanCommandSwitch("--tool-path")]
    public virtual string? ToolPath { get; set; }

    [BooleanCommandSwitch("--local")]
    public virtual bool? Local { get; set; }

    [BooleanCommandSwitch("--create-manifest-if-needed")]
    public virtual bool? CreateManifestIfNeeded { get; set; }
}

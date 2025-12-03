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

    [CliArgument(Name = "<PACKAGE_NAME>")]
    public string? PackageName { get; set; }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliOption("--arch")]
    public virtual string? Architecture { get; set; }

    [CliOption("--add-source")]
    public virtual string? AddSource { get; set; }

    [CliOption("--configfile")]
    public virtual string? Configfile { get; set; }

    [CliFlag("--disable-parallel")]
    public virtual bool? DisableParallel { get; set; }

    [CliOption("--framework")]
    public virtual string? Framework { get; set; }

    [CliFlag("--ignore-failed-sources")]
    public virtual bool? IgnoreFailedSources { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CliFlag("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CliOption("--tool-manifest")]
    public virtual string? ToolManifest { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CliOption("--version")]
    public virtual string? VersionNumber { get; set; }

    [CliFlag("--tool-path")]
    public virtual string? ToolPath { get; set; }

    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliFlag("--create-manifest-if-needed")]
    public virtual bool? CreateManifestIfNeeded { get; set; }
}

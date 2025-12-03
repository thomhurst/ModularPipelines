using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetToolUpdateOptions : DotNetOptions
{
    public DotNetToolUpdateOptions(
        string packageId,
        string toolPath
    )
    {
        CommandParts = ["tool", "update", "<PACKAGE_ID>"];

        PackageId = packageId;
        ToolPath = toolPath;
    }

    public DotNetToolUpdateOptions(
        string packageId
    )
    {
        CommandParts = ["tool", "update", "<PACKAGE_ID>"];

        PackageId = packageId;
    }

    [CliArgument(Name = "<PACKAGE_ID>")]
    public string? PackageId { get; set; }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

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

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }

    [CliFlag("--tool-path")]
    public virtual string? ToolPath { get; set; }

    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliOption("--tool-manifest")]
    public virtual string? ToolManifest { get; set; }
}

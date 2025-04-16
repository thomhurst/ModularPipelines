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

    [PositionalArgument(PlaceholderName = "<PACKAGE_ID>")]
    public string? PackageId { get; set; }

    [BooleanCommandSwitch("--global")]
    public virtual bool? Global { get; set; }

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

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CommandSwitch("--version")]
    public virtual string? Version { get; set; }

    [BooleanCommandSwitch("--tool-path")]
    public virtual string? ToolPath { get; set; }

    [BooleanCommandSwitch("--local")]
    public virtual bool? Local { get; set; }

    [CommandSwitch("--tool-manifest")]
    public virtual string? ToolManifest { get; set; }
}

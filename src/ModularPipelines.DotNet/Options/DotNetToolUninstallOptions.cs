using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetToolUninstallOptions : DotNetOptions
{
    public DotNetToolUninstallOptions(
        string packageName,
        string path
    )
    {
        CommandParts = ["tool", "uninstall", "<PACKAGE_NAME>"];

        PackageName = packageName;

        ToolPath = path;
    }

    public DotNetToolUninstallOptions(
        string packageName
    )
    {
        CommandParts = ["tool", "uninstall", "<PACKAGE_NAME>"];

        PackageName = packageName;
    }

    [CliArgument(Name = "<PACKAGE_NAME>")]
    public virtual string? PackageName { get; set; }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliFlag("--tool-path")]
    public virtual string? ToolPath { get; set; }

    [CliOption("--tool-manifest")]
    public virtual string? ToolManifest { get; set; }
}

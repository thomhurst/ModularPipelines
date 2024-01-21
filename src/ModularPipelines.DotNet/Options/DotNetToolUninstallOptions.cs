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

        Path = path;
    }

    public DotNetToolUninstallOptions(
        string packageName
    )
    {
        CommandParts = ["tool", "uninstall", "<PACKAGE_NAME>"];

        PackageName = packageName;
    }

    [PositionalArgument(PlaceholderName = "<PACKAGE_NAME>")]
    public string? PackageName { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [BooleanCommandSwitch("--tool-path")]
    public bool? ToolPath { get; set; }

    [PositionalArgument(PlaceholderName = "<PATH>")]
    public string? Path { get; set; }

    [CommandSwitch("--tool-manifest")]
    public string? ToolManifest { get; set; }
}

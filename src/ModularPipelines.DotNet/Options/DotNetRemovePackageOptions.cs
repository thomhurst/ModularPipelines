using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetRemovePackageOptions : DotNetOptions
{
    public DotNetRemovePackageOptions(
        string project,
        string packageName
    )
    {
        CommandParts = ["remove", "[<PROJECT>]", "package", "<PACKAGE_NAME>"];

        Project = project;

        PackageName = packageName;
    }

    public DotNetRemovePackageOptions(
        string packageName
    )
    {
        CommandParts = ["remove", "[<PROJECT>]", "package", "<PACKAGE_NAME>"];

        PackageName = packageName;
    }

    [PositionalArgument(PlaceholderName = "[<PROJECT>]")]
    public string? Project { get; set; }

    [PositionalArgument(PlaceholderName = "<PACKAGE_NAME>")]
    public string? PackageName { get; set; }
}

using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetToolListOptions : DotNetOptions
{
    public DotNetToolListOptions(
        string packageId
    )
    {
        CommandParts = ["tool", "list", "[<PACKAGE_ID>]"];

        PackageId = packageId;
    }

    public DotNetToolListOptions()
    {
        CommandParts = ["tool", "list"];
    }

    [PositionalArgument(PlaceholderName = "[<PACKAGE_ID>]")]
    public string? PackageId { get; set; }
}

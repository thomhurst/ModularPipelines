using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetToolListOptions : DotNetOptions
{
    public DotNetToolListOptions()
    {
        CommandParts = ["tool", "list", "[<PACKAGE_ID>]"];
    }

    public DotNetToolListOptions(
        string packageId
    )
    {
        CommandParts = ["tool", "list", "[<PACKAGE_ID>]"];

        PackageId = packageId;
    }

    [CliFlag("--global")]
    public virtual bool? Global { get; set; }

    [CliFlag("--tool-path")]
    public virtual bool? ToolPath { get; set; }

    [CliFlag("--local")]
    public virtual bool? Local { get; set; }

    [CliArgument(Name = "[<PACKAGE_ID>]")]
    public string? PackageId { get; set; }
}

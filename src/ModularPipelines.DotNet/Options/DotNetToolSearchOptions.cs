using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetToolSearchOptions : DotNetOptions
{
    public DotNetToolSearchOptions(
        string searchTerm
    )
    {
        CommandParts = ["tool", "search"];

        SearchTerm = searchTerm;
    }

    [BooleanCommandSwitch("--detail")]
    public virtual bool? Detail { get; set; }

    [BooleanCommandSwitch("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CommandSwitch("--skip")]
    public virtual string? Skip { get; set; }

    [CommandSwitch("--take")]
    public virtual string? Take { get; set; }

    [PositionalArgument(PlaceholderName = "<SEARCH TERM>")]
    public string? SearchTerm { get; set; }
}

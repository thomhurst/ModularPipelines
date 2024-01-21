using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetWorkloadSearchOptions : DotNetOptions
{
    public DotNetWorkloadSearchOptions(
        string searchString
    )
    {
        CommandParts = ["workload", "search", "[<SEARCH_STRING>]"];

        SearchString = searchString;
    }

    public DotNetWorkloadSearchOptions()
    {
        CommandParts = ["workload", "search", "[<SEARCH_STRING>]"];
    }

    [PositionalArgument(PlaceholderName = "[<SEARCH_STRING>]")]
    public string? SearchString { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }
}

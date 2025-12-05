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

    [CliArgument(Name = "[<SEARCH_STRING>]")]
    public virtual string? SearchString { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }
}

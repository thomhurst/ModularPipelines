using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("workload", "search", "[<SEARCH_STRING>]")]
[ExcludeFromCodeCoverage]
public record DotNetWorkloadSearchOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "[<SEARCH_STRING>]")]
    public string? SearchString { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }
}

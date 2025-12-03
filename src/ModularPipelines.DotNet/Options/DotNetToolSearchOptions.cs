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

    [CliFlag("--detail")]
    public virtual bool? Detail { get; set; }

    [CliFlag("--prerelease")]
    public virtual bool? Prerelease { get; set; }

    [CliOption("--skip")]
    public virtual string? Skip { get; set; }

    [CliOption("--take")]
    public virtual string? Take { get; set; }

    [CliArgument(Name = "<SEARCH TERM>")]
    public string? SearchTerm { get; set; }
}

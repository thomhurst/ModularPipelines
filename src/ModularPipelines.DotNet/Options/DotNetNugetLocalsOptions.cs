using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetLocalsOptions : DotNetOptions
{
    public DotNetNugetLocalsOptions(
        string cacheLocation
    )
    {
        CommandParts = ["nuget", "locals", "<CACHE_LOCATION>", "[(-c|--clear)|(-l|--list)]"];

        CacheLocation = cacheLocation;
    }

    [CliArgument(Name = "<CACHE_LOCATION>")]
    public string? CacheLocation { get; set; }

    [CliFlag("--clear")]
    public virtual bool? Clear { get; set; }

    [CliFlag("--list")]
    public virtual bool? List { get; set; }

    [CliFlag("--force-english-output")]
    public virtual bool? ForceEnglishOutput { get; set; }
}

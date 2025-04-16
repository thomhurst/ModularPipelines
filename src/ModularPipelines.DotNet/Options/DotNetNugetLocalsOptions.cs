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

    [PositionalArgument(PlaceholderName = "<CACHE_LOCATION>")]
    public string? CacheLocation { get; set; }

    [BooleanCommandSwitch("--clear")]
    public virtual bool? Clear { get; set; }

    [BooleanCommandSwitch("--list")]
    public virtual bool? List { get; set; }

    [BooleanCommandSwitch("--force-english-output")]
    public virtual bool? ForceEnglishOutput { get; set; }
}

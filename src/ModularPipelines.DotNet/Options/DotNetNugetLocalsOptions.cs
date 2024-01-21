using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetLocalsOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "<CACHE_LOCATION>")]
    public string? CacheLocation { get; set; }

    [BooleanCommandSwitch("--clear")]
    public bool? Clear { get; set; }

    [BooleanCommandSwitch("--list")]
    public bool? List { get; set; }

    [BooleanCommandSwitch("--force-english-output")]
    public bool? ForceEnglishOutput { get; set; }
}

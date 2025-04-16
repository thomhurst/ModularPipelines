using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetPushOptions : DotNetOptions
{
    public DotNetNugetPushOptions(
        string path
    )
    {
        CommandParts = ["nuget", "push", "[<ROOT>]"];

        Path = path;
    }

    public DotNetNugetPushOptions()
    {
        CommandParts = ["nuget", "push", "[<ROOT>]"];
    }

    [PositionalArgument(PlaceholderName = "[<ROOT>]")]
    public string? Path { get; set; }

    [BooleanCommandSwitch("--disable-buffering")]
    public virtual bool? DisableBuffering { get; set; }

    [BooleanCommandSwitch("--force-english-output")]
    public virtual bool? ForceEnglishOutput { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CommandSwitch("--api-key")]
    public virtual string? ApiKey { get; set; }

    [BooleanCommandSwitch("--no-symbols")]
    public virtual bool? NoSymbols { get; set; }

    [BooleanCommandSwitch("--no-service-endpoint")]
    public virtual bool? NoServiceEndpoint { get; set; }

    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [BooleanCommandSwitch("--skip-duplicate")]
    public virtual bool? SkipDuplicate { get; set; }

    [CommandSwitch("--symbol-api-key")]
    public virtual string? SymbolApiKey { get; set; }

    [CommandSwitch("--symbol-source")]
    public virtual string? SymbolSource { get; set; }

    [CommandSwitch("--timeout")]
    public virtual string? Timeout { get; set; }
}

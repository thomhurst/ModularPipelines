using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("nuget", "push", "[<ROOT>]")]
[ExcludeFromCodeCoverage]
public record DotNetNugetPushOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "[<ROOT>]")]
    public string? Path { get; set; }

    [BooleanCommandSwitch("--disable-buffering")]
    public bool? DisableBuffering { get; set; }

    [BooleanCommandSwitch("--force-english-output")]
    public bool? ForceEnglishOutput { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [CommandSwitch("--api-key")]
    public string? ApiKey { get; set; }

    [BooleanCommandSwitch("--no-symbols")]
    public bool? NoSymbols { get; set; }

    [BooleanCommandSwitch("--no-service-endpoint")]
    public bool? NoServiceEndpoint { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [BooleanCommandSwitch("--skip-duplicate")]
    public bool? SkipDuplicate { get; set; }

    [CommandSwitch("--symbol-api-key")]
    public string? SymbolApiKey { get; set; }

    [CommandSwitch("--symbol-source")]
    public string? SymbolSource { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}

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

    [CliArgument(Name = "[<ROOT>]")]
    public virtual string? Path { get; set; }

    [CliFlag("--disable-buffering")]
    public virtual bool? DisableBuffering { get; set; }

    [CliFlag("--force-english-output")]
    public virtual bool? ForceEnglishOutput { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliOption("--api-key")]
    public virtual string? ApiKey { get; set; }

    [CliFlag("--no-symbols")]
    public virtual bool? NoSymbols { get; set; }

    [CliFlag("--no-service-endpoint")]
    public virtual bool? NoServiceEndpoint { get; set; }

    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliFlag("--skip-duplicate")]
    public virtual bool? SkipDuplicate { get; set; }

    [CliOption("--symbol-api-key")]
    public virtual string? SymbolApiKey { get; set; }

    [CliOption("--symbol-source")]
    public virtual string? SymbolSource { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }
}

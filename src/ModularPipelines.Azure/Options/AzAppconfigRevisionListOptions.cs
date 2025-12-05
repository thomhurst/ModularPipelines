using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "revision", "list")]
public record AzAppconfigRevisionListOptions : AzOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--datetime")]
    public string? Datetime { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--fields")]
    public string? Fields { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}
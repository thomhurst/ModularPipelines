using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "remove")]
public record AzStorageRemoveOptions : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--exclude-path")]
    public string? ExcludePath { get; set; }

    [CommandSwitch("--exclude-pattern")]
    public string? ExcludePattern { get; set; }

    [CommandSwitch("--include-path")]
    public string? IncludePath { get; set; }

    [CommandSwitch("--include-pattern")]
    public string? IncludePattern { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--recursive")]
    public string? Recursive { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--share-name")]
    public string? ShareName { get; set; }
}
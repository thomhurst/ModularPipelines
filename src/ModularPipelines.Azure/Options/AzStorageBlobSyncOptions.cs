using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "sync")]
public record AzStorageBlobSyncOptions(
[property: CommandSwitch("--container")] string Container,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [BooleanCommandSwitch("--delete-destination")]
    public bool? DeleteDestination { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--exclude-path")]
    public string? ExcludePath { get; set; }

    [CommandSwitch("--exclude-pattern")]
    public string? ExcludePattern { get; set; }

    [CommandSwitch("--include-pattern")]
    public string? IncludePattern { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ExtraOptions { get; set; }
}
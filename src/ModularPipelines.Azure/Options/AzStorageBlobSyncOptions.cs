using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "blob", "sync")]
public record AzStorageBlobSyncOptions(
[property: CliOption("--container")] string Container,
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--delete-destination")]
    public bool? DeleteDestination { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--exclude-path")]
    public string? ExcludePath { get; set; }

    [CliOption("--exclude-pattern")]
    public string? ExcludePattern { get; set; }

    [CliOption("--include-pattern")]
    public string? IncludePattern { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? EXTRAOPTIONS { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("tsi", "environment", "gen1", "create")]
public record AzTsiEnvironmentGen1CreateOptions(
[property: CliOption("--data-retention-time")] string DataRetentionTime,
[property: CliOption("--environment-name")] string EnvironmentName,
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--exceeded-behavior")]
    public string? ExceededBehavior { get; set; }

    [CliOption("--key-properties")]
    public string? KeyProperties { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}
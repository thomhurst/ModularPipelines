using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "restorable-database-account", "show")]
public record AzCosmosdbRestorableDatabaseAccountShowOptions : AzOptions
{
    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "restorable-database-account", "show")]
public record AzCosmosdbRestorableDatabaseAccountShowOptions : AzOptions
{
    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}
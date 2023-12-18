using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "restorable-database-account", "show", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbRestorableDatabaseAccountShowCosmosdbPreviewExtensionOptions : AzOptions
{
    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}
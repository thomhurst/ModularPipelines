using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "restorable-database-account", "show", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbRestorableDatabaseAccountShowCosmosdbPreviewExtensionOptions : AzOptions
{
    [CliOption("--instance-id")]
    public string? InstanceId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }
}
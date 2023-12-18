using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "restorable-database-account", "list", "(cosmosdb-preview", "extension)")]
public record AzCosmosdbRestorableDatabaseAccountListCosmosdbPreviewExtensionOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}
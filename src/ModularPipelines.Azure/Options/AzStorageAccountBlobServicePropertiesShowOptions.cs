using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "blob-service-properties", "show")]
public record AzStorageAccountBlobServicePropertiesShowOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
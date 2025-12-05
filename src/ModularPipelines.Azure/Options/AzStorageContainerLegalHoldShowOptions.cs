using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "container", "legal-hold", "show")]
public record AzStorageContainerLegalHoldShowOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--container-name")] string ContainerName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
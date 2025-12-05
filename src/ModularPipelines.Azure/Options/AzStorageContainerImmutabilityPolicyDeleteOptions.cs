using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "container", "immutability-policy", "delete")]
public record AzStorageContainerImmutabilityPolicyDeleteOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--if-match")] string IfMatch
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
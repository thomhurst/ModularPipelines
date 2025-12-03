using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "container", "legal-hold", "clear")]
public record AzStorageContainerLegalHoldClearOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--tags")] string Tags
) : AzOptions
{
    [CliFlag("--allow-protected-append-writes-all")]
    public bool? AllowProtectedAppendWritesAll { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
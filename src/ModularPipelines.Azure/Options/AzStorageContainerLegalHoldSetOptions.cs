using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "container", "legal-hold", "set")]
public record AzStorageContainerLegalHoldSetOptions(
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
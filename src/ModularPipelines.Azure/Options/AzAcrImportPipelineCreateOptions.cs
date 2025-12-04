using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "import-pipeline", "create")]
public record AzAcrImportPipelineCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--secret-uri")] string SecretUri,
[property: CliOption("--storage-container-uri")] string StorageContainerUri
) : AzOptions
{
    [CliOption("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CliOption("--options")]
    public string? Options { get; set; }

    [CliFlag("--source-trigger-enabled")]
    public bool? SourceTriggerEnabled { get; set; }
}
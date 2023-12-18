using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "import-pipeline", "create")]
public record AzAcrImportPipelineCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--secret-uri")] string SecretUri,
[property: CommandSwitch("--storage-container-uri")] string StorageContainerUri
) : AzOptions
{
    [CommandSwitch("--assign-identity")]
    public string? AssignIdentity { get; set; }

    [CommandSwitch("--options")]
    public string? Options { get; set; }

    [BooleanCommandSwitch("--source-trigger-enabled")]
    public bool? SourceTriggerEnabled { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "datastore", "upload")]
public record AzMlDatastoreUploadOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--src-path")] string SrcPath
) : AzOptions
{
    [CommandSwitch("--hide-progress")]
    public string? HideProgress { get; set; }

    [CommandSwitch("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CommandSwitch("--overwrite")]
    public string? Overwrite { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--target-path")]
    public string? TargetPath { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}
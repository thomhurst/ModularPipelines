using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "model", "profile")]
public record AzMlModelProfileOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--base-image")]
    public string? BaseImage { get; set; }

    [CommandSwitch("--base-image-registry")]
    public string? BaseImageRegistry { get; set; }

    [CommandSwitch("--cc")]
    public string? Cc { get; set; }

    [CommandSwitch("--cf")]
    public string? Cf { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--ed")]
    public string? Ed { get; set; }

    [CommandSwitch("--entry-script")]
    public string? EntryScript { get; set; }

    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--environment-version")]
    public string? EnvironmentVersion { get; set; }

    [CommandSwitch("--gb")]
    public string? Gb { get; set; }

    [CommandSwitch("--ic")]
    public string? Ic { get; set; }

    [CommandSwitch("--idi")]
    public string? Idi { get; set; }

    [CommandSwitch("--model")]
    public string? Model { get; set; }

    [CommandSwitch("--model-metadata-file")]
    public string? ModelMetadataFile { get; set; }

    [CommandSwitch("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sd")]
    public string? Sd { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("-v")]
    public string? V { get; set; }
}
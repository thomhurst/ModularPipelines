using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "model", "package")]
public record AzMlModelPackageOptions : AzOptions
{
    [CommandSwitch("--cf")]
    public string? Cf { get; set; }

    [CommandSwitch("--ed")]
    public string? Ed { get; set; }

    [CommandSwitch("--entry-script")]
    public string? EntryScript { get; set; }

    [CommandSwitch("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CommandSwitch("--environment-version")]
    public string? EnvironmentVersion { get; set; }

    [CommandSwitch("--ic")]
    public string? Ic { get; set; }

    [CommandSwitch("--il")]
    public string? Il { get; set; }

    [CommandSwitch("--image-name")]
    public string? ImageName { get; set; }

    [CommandSwitch("--model")]
    public string? Model { get; set; }

    [CommandSwitch("--model-metadata-file")]
    public string? ModelMetadataFile { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--output-path")]
    public string? OutputPath { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rt")]
    public string? Rt { get; set; }

    [CommandSwitch("--sd")]
    public string? Sd { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CommandSwitch("-v")]
    public string? V { get; set; }
}
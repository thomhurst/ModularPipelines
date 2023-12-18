using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "create")]
public record AzArcdataDcCreateOptions(
[property: CommandSwitch("--connectivity-mode")] string ConnectivityMode,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--annotations")]
    public string? Annotations { get; set; }

    [BooleanCommandSwitch("--auto-upload-logs")]
    public bool? AutoUploadLogs { get; set; }

    [BooleanCommandSwitch("--auto-upload-metrics")]
    public bool? AutoUploadMetrics { get; set; }

    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [CommandSwitch("--custom-location")]
    public string? CustomLocation { get; set; }

    [CommandSwitch("--image-tag")]
    public string? ImageTag { get; set; }

    [CommandSwitch("--infrastructure")]
    public string? Infrastructure { get; set; }

    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--logs-ui-private-key-file")]
    public string? LogsUiPrivateKeyFile { get; set; }

    [CommandSwitch("--logs-ui-public-key-file")]
    public string? LogsUiPublicKeyFile { get; set; }

    [CommandSwitch("--metrics-ui-private-key-file")]
    public string? MetricsUiPrivateKeyFile { get; set; }

    [CommandSwitch("--metrics-ui-public-key-file")]
    public string? MetricsUiPublicKeyFile { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--service-annotations")]
    public string? ServiceAnnotations { get; set; }

    [CommandSwitch("--service-labels")]
    public string? ServiceLabels { get; set; }

    [CommandSwitch("--storage-annotations")]
    public string? StorageAnnotations { get; set; }

    [CommandSwitch("--storage-class")]
    public string? StorageClass { get; set; }

    [CommandSwitch("--storage-labels")]
    public string? StorageLabels { get; set; }

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcdata", "dc", "create")]
public record AzArcdataDcCreateOptions(
[property: CliOption("--connectivity-mode")] string ConnectivityMode,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--annotations")]
    public string? Annotations { get; set; }

    [CliFlag("--auto-upload-logs")]
    public bool? AutoUploadLogs { get; set; }

    [CliFlag("--auto-upload-metrics")]
    public bool? AutoUploadMetrics { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--custom-location")]
    public string? CustomLocation { get; set; }

    [CliOption("--image-tag")]
    public string? ImageTag { get; set; }

    [CliOption("--infrastructure")]
    public string? Infrastructure { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--logs-ui-private-key-file")]
    public string? LogsUiPrivateKeyFile { get; set; }

    [CliOption("--logs-ui-public-key-file")]
    public string? LogsUiPublicKeyFile { get; set; }

    [CliOption("--metrics-ui-private-key-file")]
    public string? MetricsUiPrivateKeyFile { get; set; }

    [CliOption("--metrics-ui-public-key-file")]
    public string? MetricsUiPublicKeyFile { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--service-annotations")]
    public string? ServiceAnnotations { get; set; }

    [CliOption("--service-labels")]
    public string? ServiceLabels { get; set; }

    [CliOption("--storage-annotations")]
    public string? StorageAnnotations { get; set; }

    [CliOption("--storage-class")]
    public string? StorageClass { get; set; }

    [CliOption("--storage-labels")]
    public string? StorageLabels { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}
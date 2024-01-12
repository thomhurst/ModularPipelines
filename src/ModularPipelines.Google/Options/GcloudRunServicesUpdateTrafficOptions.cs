using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("run", "services", "update-traffic")]
public record GcloudRunServicesUpdateTrafficOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Namespace
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [BooleanCommandSwitch("--clear-tags")]
    public bool? ClearTags { get; set; }

    [CommandSwitch("--set-tags")]
    public string[]? SetTags { get; set; }

    [CommandSwitch("--remove-tags")]
    public string[]? RemoveTags { get; set; }

    [CommandSwitch("--update-tags")]
    public string[]? UpdateTags { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [CommandSwitch("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--kubeconfig")]
    public string? Kubeconfig { get; set; }

    [BooleanCommandSwitch("--to-latest")]
    public bool? ToLatest { get; set; }

    [CommandSwitch("--to-revisions")]
    public string[]? ToRevisions { get; set; }

    [CommandSwitch("--to-tags")]
    public string[]? ToTags { get; set; }
}
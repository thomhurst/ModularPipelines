using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi-arc", "edit")]
public record AzSqlMiArcEditOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--agent-enabled")]
    public bool? AgentEnabled { get; set; }

    [CommandSwitch("--annotations")]
    public string? Annotations { get; set; }

    [CommandSwitch("--cores-limit")]
    public string? CoresLimit { get; set; }

    [CommandSwitch("--cores-request")]
    public string? CoresRequest { get; set; }

    [CommandSwitch("--custom-location")]
    public string? CustomLocation { get; set; }

    [CommandSwitch("--dev")]
    public string? Dev { get; set; }

    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--memory-limit")]
    public string? MemoryLimit { get; set; }

    [CommandSwitch("--memory-request")]
    public string? MemoryRequest { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--retention-days")]
    public string? RetentionDays { get; set; }

    [CommandSwitch("--service-annotations")]
    public string? ServiceAnnotations { get; set; }

    [CommandSwitch("--service-labels")]
    public string? ServiceLabels { get; set; }

    [CommandSwitch("--tag-name")]
    public string? TagName { get; set; }

    [CommandSwitch("--tag-value")]
    public string? TagValue { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--time-zone")]
    public string? TimeZone { get; set; }

    [CommandSwitch("--trace-flags")]
    public string? TraceFlags { get; set; }

    [CommandSwitch("--use-k8s")]
    public string? UseK8s { get; set; }
}
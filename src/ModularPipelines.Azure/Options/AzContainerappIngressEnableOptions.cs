using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "ingress", "enable")]
public record AzContainerappIngressEnableOptions(
[property: CommandSwitch("--target-port")] string TargetPort,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [BooleanCommandSwitch("--allow-insecure")]
    public bool? AllowInsecure { get; set; }

    [CommandSwitch("--exposed-port")]
    public string? ExposedPort { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--transport")]
    public string? Transport { get; set; }
}
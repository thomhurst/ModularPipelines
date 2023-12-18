using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "job", "start")]
public record AzContainerappJobStartOptions : AzOptions
{
    [CommandSwitch("--args")]
    public string? Args { get; set; }

    [CommandSwitch("--command")]
    public string? Command { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--cpu")]
    public string? Cpu { get; set; }

    [CommandSwitch("--env-vars")]
    public string? EnvVars { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--image")]
    public string? Image { get; set; }

    [CommandSwitch("--memory")]
    public string? Memory { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--registry-identity")]
    public string? RegistryIdentity { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--yaml")]
    public string? Yaml { get; set; }
}
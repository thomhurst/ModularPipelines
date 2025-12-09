using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "job", "start")]
public record AzContainerappJobStartOptions : AzOptions
{
    [CliOption("--args")]
    public string? Args { get; set; }

    [CliOption("--command")]
    public string? Command { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--cpu")]
    public string? Cpu { get; set; }

    [CliOption("--env-vars")]
    public string? EnvVars { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--image")]
    public string? Image { get; set; }

    [CliOption("--memory")]
    public string? Memory { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--registry-identity")]
    public string? RegistryIdentity { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--yaml")]
    public string? Yaml { get; set; }
}
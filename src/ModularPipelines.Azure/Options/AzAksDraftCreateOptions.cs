using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "draft", "create")]
public record AzAksDraftCreateOptions : AzOptions
{
    [CommandSwitch("--app")]
    public string? App { get; set; }

    [CommandSwitch("--create-config")]
    public string? CreateConfig { get; set; }

    [CommandSwitch("--deployment-only")]
    public string? DeploymentOnly { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--dockerfile-only")]
    public string? DockerfileOnly { get; set; }

    [CommandSwitch("--language")]
    public string? Language { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }
}
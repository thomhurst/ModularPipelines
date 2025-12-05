using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "draft", "create")]
public record AzAksDraftCreateOptions : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--create-config")]
    public string? CreateConfig { get; set; }

    [CliOption("--deployment-only")]
    public string? DeploymentOnly { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--dockerfile-only")]
    public string? DockerfileOnly { get; set; }

    [CliOption("--language")]
    public string? Language { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }
}
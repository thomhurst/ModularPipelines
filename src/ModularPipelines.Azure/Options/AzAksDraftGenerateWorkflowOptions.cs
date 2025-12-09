using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "draft", "generate-workflow")]
public record AzAksDraftGenerateWorkflowOptions : AzOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--registry-name")]
    public string? RegistryName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
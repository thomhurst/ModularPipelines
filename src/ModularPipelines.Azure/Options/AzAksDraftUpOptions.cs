using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "draft", "up")]
public record AzAksDraftUpOptions : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--gh-repo")]
    public string? GhRepo { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--registry-name")]
    public string? RegistryName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }
}
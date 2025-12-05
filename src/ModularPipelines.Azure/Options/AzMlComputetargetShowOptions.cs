using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "computetarget", "show")]
public record AzMlComputetargetShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }

    [CliFlag("-v")]
    public bool? V { get; set; }
}
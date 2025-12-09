using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "namespace", "application-group", "show")]
public record AzEventhubsNamespaceApplicationGroupShowOptions : AzOptions
{
    [CliOption("--application-group-name")]
    public string? ApplicationGroupName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
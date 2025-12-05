using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "namespace", "application-group", "create")]
public record AzEventhubsNamespaceApplicationGroupCreateOptions(
[property: CliOption("--client-app-group-id")] string ClientAppGroupId,
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--is-enabled")]
    public bool? IsEnabled { get; set; }

    [CliOption("--policy-config")]
    public string? PolicyConfig { get; set; }
}
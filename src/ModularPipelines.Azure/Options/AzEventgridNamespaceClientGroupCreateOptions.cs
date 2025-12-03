using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "namespace", "client-group", "create")]
public record AzEventgridNamespaceClientGroupCreateOptions(
[property: CliOption("--client-group-name")] string ClientGroupName,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--group-query")]
    public string? GroupQuery { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}
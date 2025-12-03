using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datafactory", "linked-service", "create")]
public record AzDatafactoryLinkedServiceCreateOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--linked-service-name")] string LinkedServiceName,
[property: CliOption("--properties")] string Properties,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "start-domain-maintenance")]
public record AwsOpensearchStartDomainMaintenanceOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--action")] string Action
) : AwsOptions
{
    [CliOption("--node-id")]
    public string? NodeId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
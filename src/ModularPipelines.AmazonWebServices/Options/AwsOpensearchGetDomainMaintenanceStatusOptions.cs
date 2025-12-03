using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearch", "get-domain-maintenance-status")]
public record AwsOpensearchGetDomainMaintenanceStatusOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--maintenance-id")] string MaintenanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
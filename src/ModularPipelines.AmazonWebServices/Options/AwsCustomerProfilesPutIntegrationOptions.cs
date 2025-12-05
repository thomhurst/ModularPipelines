using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("customer-profiles", "put-integration")]
public record AwsCustomerProfilesPutIntegrationOptions(
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--uri")]
    public string? Uri { get; set; }

    [CliOption("--object-type-name")]
    public string? ObjectTypeName { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--flow-definition")]
    public string? FlowDefinition { get; set; }

    [CliOption("--object-type-names")]
    public IEnumerable<KeyValue>? ObjectTypeNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
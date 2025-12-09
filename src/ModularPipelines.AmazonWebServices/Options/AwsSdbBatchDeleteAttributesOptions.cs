using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sdb", "batch-delete-attributes")]
public record AwsSdbBatchDeleteAttributesOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--items")] string[] Items
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
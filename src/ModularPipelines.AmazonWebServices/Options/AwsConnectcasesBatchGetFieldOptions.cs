using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectcases", "batch-get-field")]
public record AwsConnectcasesBatchGetFieldOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--fields")] string[] Fields
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
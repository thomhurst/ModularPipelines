using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "delete-domain")]
public record AwsSagemakerDeleteDomainOptions(
[property: CliOption("--domain-id")] string DomainId
) : AwsOptions
{
    [CliOption("--retention-policy")]
    public string? RetentionPolicy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
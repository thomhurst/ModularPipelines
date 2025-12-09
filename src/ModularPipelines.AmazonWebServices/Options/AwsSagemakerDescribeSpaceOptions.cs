using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-space")]
public record AwsSagemakerDescribeSpaceOptions(
[property: CliOption("--domain-id")] string DomainId,
[property: CliOption("--space-name")] string SpaceName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
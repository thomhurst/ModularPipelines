using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "batch-describe-type-configurations")]
public record AwsCloudformationBatchDescribeTypeConfigurationsOptions(
[property: CliOption("--type-configuration-identifiers")] string[] TypeConfigurationIdentifiers
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
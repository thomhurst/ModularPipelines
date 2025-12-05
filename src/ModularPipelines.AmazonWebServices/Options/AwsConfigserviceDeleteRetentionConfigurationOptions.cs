using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "delete-retention-configuration")]
public record AwsConfigserviceDeleteRetentionConfigurationOptions(
[property: CliOption("--retention-configuration-name")] string RetentionConfigurationName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
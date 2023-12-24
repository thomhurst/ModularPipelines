using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "batch-describe-type-configurations")]
public record AwsCloudformationBatchDescribeTypeConfigurationsOptions(
[property: CommandSwitch("--type-configuration-identifiers")] string[] TypeConfigurationIdentifiers
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
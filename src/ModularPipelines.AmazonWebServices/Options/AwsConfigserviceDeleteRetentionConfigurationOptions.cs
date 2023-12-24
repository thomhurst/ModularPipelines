using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "delete-retention-configuration")]
public record AwsConfigserviceDeleteRetentionConfigurationOptions(
[property: CommandSwitch("--retention-configuration-name")] string RetentionConfigurationName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
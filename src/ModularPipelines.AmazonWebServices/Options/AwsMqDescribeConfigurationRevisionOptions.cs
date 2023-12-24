using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mq", "describe-configuration-revision")]
public record AwsMqDescribeConfigurationRevisionOptions(
[property: CommandSwitch("--configuration-id")] string ConfigurationId,
[property: CommandSwitch("--configuration-revision")] string ConfigurationRevision
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "update-application-resource-lifecycle")]
public record AwsElasticbeanstalkUpdateApplicationResourceLifecycleOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--resource-lifecycle-config")] string ResourceLifecycleConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
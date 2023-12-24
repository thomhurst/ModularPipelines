using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "register-publisher")]
public record AwsCloudformationRegisterPublisherOptions : AwsOptions
{
    [CommandSwitch("--connection-arn")]
    public string? ConnectionArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "register-publisher")]
public record AwsCloudformationRegisterPublisherOptions : AwsOptions
{
    [CliOption("--connection-arn")]
    public string? ConnectionArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
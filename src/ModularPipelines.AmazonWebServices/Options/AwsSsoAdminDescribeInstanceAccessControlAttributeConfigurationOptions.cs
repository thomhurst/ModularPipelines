using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "describe-instance-access-control-attribute-configuration")]
public record AwsSsoAdminDescribeInstanceAccessControlAttributeConfigurationOptions(
[property: CliOption("--instance-arn")] string InstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
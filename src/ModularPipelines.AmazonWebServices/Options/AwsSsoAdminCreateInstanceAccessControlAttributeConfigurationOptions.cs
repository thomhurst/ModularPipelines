using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "create-instance-access-control-attribute-configuration")]
public record AwsSsoAdminCreateInstanceAccessControlAttributeConfigurationOptions(
[property: CliOption("--instance-access-control-attribute-configuration")] string InstanceAccessControlAttributeConfiguration,
[property: CliOption("--instance-arn")] string InstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
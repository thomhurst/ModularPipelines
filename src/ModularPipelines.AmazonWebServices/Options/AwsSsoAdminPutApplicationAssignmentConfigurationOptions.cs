using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso-admin", "put-application-assignment-configuration")]
public record AwsSsoAdminPutApplicationAssignmentConfigurationOptions(
[property: CliOption("--application-arn")] string ApplicationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "rollback-stack")]
public record AwsCloudformationRollbackStackOptions(
[property: CliOption("--stack-name")] string StackName
) : AwsOptions
{
    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
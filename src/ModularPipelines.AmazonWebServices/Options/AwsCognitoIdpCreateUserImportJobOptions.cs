using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "create-user-import-job")]
public record AwsCognitoIdpCreateUserImportJobOptions(
[property: CliOption("--job-name")] string JobName,
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--cloud-watch-logs-role-arn")] string CloudWatchLogsRoleArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
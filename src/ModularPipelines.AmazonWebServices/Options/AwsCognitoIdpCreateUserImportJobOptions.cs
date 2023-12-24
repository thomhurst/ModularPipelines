using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "create-user-import-job")]
public record AwsCognitoIdpCreateUserImportJobOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--cloud-watch-logs-role-arn")] string CloudWatchLogsRoleArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
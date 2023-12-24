using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("greengrass", "start-bulk-deployment")]
public record AwsGreengrassStartBulkDeploymentOptions(
[property: CommandSwitch("--execution-role-arn")] string ExecutionRoleArn,
[property: CommandSwitch("--input-file-uri")] string InputFileUri
) : AwsOptions
{
    [CommandSwitch("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
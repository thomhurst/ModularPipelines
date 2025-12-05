using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("greengrass", "start-bulk-deployment")]
public record AwsGreengrassStartBulkDeploymentOptions(
[property: CliOption("--execution-role-arn")] string ExecutionRoleArn,
[property: CliOption("--input-file-uri")] string InputFileUri
) : AwsOptions
{
    [CliOption("--amzn-client-token")]
    public string? AmznClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
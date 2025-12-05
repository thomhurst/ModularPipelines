using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "update-storage-system")]
public record AwsDatasyncUpdateStorageSystemOptions(
[property: CliOption("--storage-system-arn")] string StorageSystemArn
) : AwsOptions
{
    [CliOption("--server-configuration")]
    public string? ServerConfiguration { get; set; }

    [CliOption("--agent-arns")]
    public string[]? AgentArns { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--cloud-watch-log-group-arn")]
    public string? CloudWatchLogGroupArn { get; set; }

    [CliOption("--credentials")]
    public string? Credentials { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
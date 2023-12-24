using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "update-storage-system")]
public record AwsDatasyncUpdateStorageSystemOptions(
[property: CommandSwitch("--storage-system-arn")] string StorageSystemArn
) : AwsOptions
{
    [CommandSwitch("--server-configuration")]
    public string? ServerConfiguration { get; set; }

    [CommandSwitch("--agent-arns")]
    public string[]? AgentArns { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--cloud-watch-log-group-arn")]
    public string? CloudWatchLogGroupArn { get; set; }

    [CommandSwitch("--credentials")]
    public string? Credentials { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
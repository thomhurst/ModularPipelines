using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sqs", "add-permission")]
public record AwsSqsAddPermissionOptions(
[property: CommandSwitch("--queue-url")] string QueueUrl,
[property: CommandSwitch("--label")] string Label,
[property: CommandSwitch("--aws-account-ids")] string[] AwsAccountIds,
[property: CommandSwitch("--actions")] string[] Actions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
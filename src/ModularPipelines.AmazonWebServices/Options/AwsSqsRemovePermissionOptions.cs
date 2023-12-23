using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sqs", "remove-permission")]
public record AwsSqsRemovePermissionOptions(
[property: CommandSwitch("--queue-url")] string QueueUrl,
[property: CommandSwitch("--label")] string Label
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
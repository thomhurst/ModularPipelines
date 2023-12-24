using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mq", "describe-user")]
public record AwsMqDescribeUserOptions(
[property: CommandSwitch("--broker-id")] string BrokerId,
[property: CommandSwitch("--username")] string Username
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
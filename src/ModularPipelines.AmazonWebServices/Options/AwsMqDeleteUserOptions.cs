using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mq", "delete-user")]
public record AwsMqDeleteUserOptions(
[property: CommandSwitch("--broker-id")] string BrokerId,
[property: CommandSwitch("--username")] string Username
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
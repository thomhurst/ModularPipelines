using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mq", "create-user")]
public record AwsMqCreateUserOptions(
[property: CommandSwitch("--broker-id")] string BrokerId,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--username")] string Username
) : AwsOptions
{
    [CommandSwitch("--groups")]
    public string[]? Groups { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mq", "update-user")]
public record AwsMqUpdateUserOptions(
[property: CommandSwitch("--broker-id")] string BrokerId,
[property: CommandSwitch("--username")] string Username
) : AwsOptions
{
    [CommandSwitch("--groups")]
    public string[]? Groups { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sns", "create-platform-endpoint")]
public record AwsSnsCreatePlatformEndpointOptions(
[property: CommandSwitch("--platform-application-arn")] string PlatformApplicationArn,
[property: CommandSwitch("--token")] string Token
) : AwsOptions
{
    [CommandSwitch("--custom-user-data")]
    public string? CustomUserData { get; set; }

    [CommandSwitch("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "create-user")]
public record AwsElasticacheCreateUserOptions(
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--engine")] string Engine,
[property: CommandSwitch("--access-string")] string AccessString
) : AwsOptions
{
    [CommandSwitch("--passwords")]
    public string[]? Passwords { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--authentication-mode")]
    public string? AuthenticationMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
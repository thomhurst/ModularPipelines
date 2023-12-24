using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "modify-user")]
public record AwsElasticacheModifyUserOptions(
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--access-string")]
    public string? AccessString { get; set; }

    [CommandSwitch("--append-access-string")]
    public string? AppendAccessString { get; set; }

    [CommandSwitch("--passwords")]
    public string[]? Passwords { get; set; }

    [CommandSwitch("--authentication-mode")]
    public string? AuthenticationMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
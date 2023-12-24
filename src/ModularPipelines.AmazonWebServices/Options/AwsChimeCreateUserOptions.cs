using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "create-user")]
public record AwsChimeCreateUserOptions(
[property: CommandSwitch("--account-id")] string AccountId
) : AwsOptions
{
    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--email")]
    public string? Email { get; set; }

    [CommandSwitch("--user-type")]
    public string? UserType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
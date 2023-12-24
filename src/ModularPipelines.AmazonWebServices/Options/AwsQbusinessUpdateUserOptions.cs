using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "update-user")]
public record AwsQbusinessUpdateUserOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--user-aliases-to-delete")]
    public string[]? UserAliasesToDelete { get; set; }

    [CommandSwitch("--user-aliases-to-update")]
    public string[]? UserAliasesToUpdate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
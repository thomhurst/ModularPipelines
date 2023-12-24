using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "reset-password")]
public record AwsWorkmailResetPasswordOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--password")] string Password
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
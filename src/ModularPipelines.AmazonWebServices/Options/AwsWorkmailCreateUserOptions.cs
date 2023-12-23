using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "create-user")]
public record AwsWorkmailCreateUserOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--display-name")] string DisplayName
) : AwsOptions
{
    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--first-name")]
    public string? FirstName { get; set; }

    [CommandSwitch("--last-name")]
    public string? LastName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
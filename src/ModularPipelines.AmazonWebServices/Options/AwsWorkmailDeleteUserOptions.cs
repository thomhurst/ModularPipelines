using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "delete-user")]
public record AwsWorkmailDeleteUserOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
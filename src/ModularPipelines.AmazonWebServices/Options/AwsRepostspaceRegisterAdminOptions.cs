using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repostspace", "register-admin")]
public record AwsRepostspaceRegisterAdminOptions(
[property: CommandSwitch("--admin-id")] string AdminId,
[property: CommandSwitch("--space-id")] string SpaceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
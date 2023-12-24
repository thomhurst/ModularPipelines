using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "update-image-permissions")]
public record AwsAppstreamUpdateImagePermissionsOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--shared-account-id")] string SharedAccountId,
[property: CommandSwitch("--image-permissions")] string ImagePermissions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
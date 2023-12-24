using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "delete-image-permissions")]
public record AwsAppstreamDeleteImagePermissionsOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--shared-account-id")] string SharedAccountId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
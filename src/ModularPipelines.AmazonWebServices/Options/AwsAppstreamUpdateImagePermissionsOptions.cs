using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "update-image-permissions")]
public record AwsAppstreamUpdateImagePermissionsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--shared-account-id")] string SharedAccountId,
[property: CliOption("--image-permissions")] string ImagePermissions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
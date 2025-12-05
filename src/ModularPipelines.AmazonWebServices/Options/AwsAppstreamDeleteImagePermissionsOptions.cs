using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "delete-image-permissions")]
public record AwsAppstreamDeleteImagePermissionsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--shared-account-id")] string SharedAccountId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
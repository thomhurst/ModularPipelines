using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "put-mobile-device-access-override")]
public record AwsWorkmailPutMobileDeviceAccessOverrideOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--effect")] string Effect
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
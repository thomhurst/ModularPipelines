using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "get-mobile-device-access-override")]
public record AwsWorkmailGetMobileDeviceAccessOverrideOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--device-id")] string DeviceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
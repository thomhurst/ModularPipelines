using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-routing-profile")]
public record AwsConnectDeleteRoutingProfileOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--routing-profile-id")] string RoutingProfileId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
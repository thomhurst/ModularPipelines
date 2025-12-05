using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-routing-profile-concurrency")]
public record AwsConnectUpdateRoutingProfileConcurrencyOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--routing-profile-id")] string RoutingProfileId,
[property: CliOption("--media-concurrencies")] string[] MediaConcurrencies
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "enable-federation")]
public record AwsCloudtrailEnableFederationOptions(
[property: CliOption("--event-data-store")] string EventDataStore,
[property: CliOption("--federation-role-arn")] string FederationRoleArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
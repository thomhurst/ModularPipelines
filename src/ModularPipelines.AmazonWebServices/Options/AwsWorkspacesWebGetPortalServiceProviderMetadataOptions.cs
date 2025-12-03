using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspaces-web", "get-portal-service-provider-metadata")]
public record AwsWorkspacesWebGetPortalServiceProviderMetadataOptions(
[property: CliOption("--portal-arn")] string PortalArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
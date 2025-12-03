using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "start-organization-service-access-update")]
public record AwsNetworkmanagerStartOrganizationServiceAccessUpdateOptions(
[property: CliOption("--action")] string Action
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
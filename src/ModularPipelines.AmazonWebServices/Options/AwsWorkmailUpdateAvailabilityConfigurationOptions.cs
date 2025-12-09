using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "update-availability-configuration")]
public record AwsWorkmailUpdateAvailabilityConfigurationOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--ews-provider")]
    public string? EwsProvider { get; set; }

    [CliOption("--lambda-provider")]
    public string? LambdaProvider { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
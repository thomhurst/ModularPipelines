using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "test-availability-configuration")]
public record AwsWorkmailTestAvailabilityConfigurationOptions(
[property: CliOption("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CliOption("--domain-name")]
    public string? DomainName { get; set; }

    [CliOption("--ews-provider")]
    public string? EwsProvider { get; set; }

    [CliOption("--lambda-provider")]
    public string? LambdaProvider { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
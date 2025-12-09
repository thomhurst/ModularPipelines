using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "list-type-registrations")]
public record AwsCloudformationListTypeRegistrationsOptions : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--type-name")]
    public string? TypeName { get; set; }

    [CliOption("--type-arn")]
    public string? TypeArn { get; set; }

    [CliOption("--registration-status-filter")]
    public string? RegistrationStatusFilter { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
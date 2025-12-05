using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "list-type-versions")]
public record AwsCloudformationListTypeVersionsOptions : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--type-name")]
    public string? TypeName { get; set; }

    [CliOption("--arn")]
    public string? Arn { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--deprecated-status")]
    public string? DeprecatedStatus { get; set; }

    [CliOption("--publisher-id")]
    public string? PublisherId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
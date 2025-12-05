using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "describe-type")]
public record AwsCloudformationDescribeTypeOptions : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--type-name")]
    public string? TypeName { get; set; }

    [CliOption("--arn")]
    public string? Arn { get; set; }

    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--publisher-id")]
    public string? PublisherId { get; set; }

    [CliOption("--public-version-number")]
    public string? PublicVersionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
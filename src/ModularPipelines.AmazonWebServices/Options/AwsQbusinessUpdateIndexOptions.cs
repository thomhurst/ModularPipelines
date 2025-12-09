using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "update-index")]
public record AwsQbusinessUpdateIndexOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--index-id")] string IndexId
) : AwsOptions
{
    [CliOption("--capacity-configuration")]
    public string? CapacityConfiguration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--document-attribute-configurations")]
    public string[]? DocumentAttributeConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
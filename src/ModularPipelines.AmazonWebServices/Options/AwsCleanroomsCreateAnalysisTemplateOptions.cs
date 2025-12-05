using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "create-analysis-template")]
public record AwsCleanroomsCreateAnalysisTemplateOptions(
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--name")] string Name,
[property: CliOption("--format")] string Format,
[property: CliOption("--source")] string Source
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--analysis-parameters")]
    public string[]? AnalysisParameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
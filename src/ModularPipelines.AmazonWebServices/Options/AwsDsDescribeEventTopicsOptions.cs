using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "describe-event-topics")]
public record AwsDsDescribeEventTopicsOptions : AwsOptions
{
    [CliOption("--directory-id")]
    public string? DirectoryId { get; set; }

    [CliOption("--topic-names")]
    public string[]? TopicNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
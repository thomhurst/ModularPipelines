using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "describe-event-topics")]
public record AwsDsDescribeEventTopicsOptions : AwsOptions
{
    [CommandSwitch("--directory-id")]
    public string? DirectoryId { get; set; }

    [CommandSwitch("--topic-names")]
    public string[]? TopicNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "update-tags-for-resource")]
public record AwsElasticbeanstalkUpdateTagsForResourceOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CommandSwitch("--tags-to-add")]
    public string[]? TagsToAdd { get; set; }

    [CommandSwitch("--tags-to-remove")]
    public string[]? TagsToRemove { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
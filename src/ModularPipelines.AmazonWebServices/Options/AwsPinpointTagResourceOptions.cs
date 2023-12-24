using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "tag-resource")]
public record AwsPinpointTagResourceOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn,
[property: CommandSwitch("--tags-model")] string TagsModel
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
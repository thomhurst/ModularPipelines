using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("resourcegroupstaggingapi", "tag-resources")]
public record AwsResourcegroupstaggingapiTagResourcesOptions(
[property: CommandSwitch("--resource-arn-list")] string[] ResourceArnList,
[property: CommandSwitch("--tags")] IEnumerable<KeyValue> Tags
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
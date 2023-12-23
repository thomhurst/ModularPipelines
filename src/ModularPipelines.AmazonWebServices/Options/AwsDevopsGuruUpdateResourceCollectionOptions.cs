using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops-guru", "update-resource-collection")]
public record AwsDevopsGuruUpdateResourceCollectionOptions(
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--resource-collection")] string ResourceCollection
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
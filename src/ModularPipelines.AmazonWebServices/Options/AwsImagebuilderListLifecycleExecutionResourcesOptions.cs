using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "list-lifecycle-execution-resources")]
public record AwsImagebuilderListLifecycleExecutionResourcesOptions(
[property: CommandSwitch("--lifecycle-execution-id")] string LifecycleExecutionId
) : AwsOptions
{
    [CommandSwitch("--parent-resource-id")]
    public string? ParentResourceId { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
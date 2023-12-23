using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("entityresolution", "update-matching-workflow")]
public record AwsEntityresolutionUpdateMatchingWorkflowOptions(
[property: CommandSwitch("--input-source-config")] string[] InputSourceConfig,
[property: CommandSwitch("--output-source-config")] string[] OutputSourceConfig,
[property: CommandSwitch("--resolution-techniques")] string ResolutionTechniques,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--workflow-name")] string WorkflowName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--incremental-run-config")]
    public string? IncrementalRunConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
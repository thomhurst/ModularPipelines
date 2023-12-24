using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("entityresolution", "update-id-mapping-workflow")]
public record AwsEntityresolutionUpdateIdMappingWorkflowOptions(
[property: CommandSwitch("--id-mapping-techniques")] string IdMappingTechniques,
[property: CommandSwitch("--input-source-config")] string[] InputSourceConfig,
[property: CommandSwitch("--output-source-config")] string[] OutputSourceConfig,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--workflow-name")] string WorkflowName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("entityresolution", "update-id-mapping-workflow")]
public record AwsEntityresolutionUpdateIdMappingWorkflowOptions(
[property: CliOption("--id-mapping-techniques")] string IdMappingTechniques,
[property: CliOption("--input-source-config")] string[] InputSourceConfig,
[property: CliOption("--output-source-config")] string[] OutputSourceConfig,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--workflow-name")] string WorkflowName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
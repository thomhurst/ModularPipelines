using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-task-template")]
public record AwsConnectDeleteTaskTemplateOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--task-template-id")] string TaskTemplateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
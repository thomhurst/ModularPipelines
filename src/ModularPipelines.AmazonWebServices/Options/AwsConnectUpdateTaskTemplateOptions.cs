using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-task-template")]
public record AwsConnectUpdateTaskTemplateOptions(
[property: CliOption("--task-template-id")] string TaskTemplateId,
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--contact-flow-id")]
    public string? ContactFlowId { get; set; }

    [CliOption("--constraints")]
    public string? Constraints { get; set; }

    [CliOption("--defaults")]
    public string? Defaults { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--fields")]
    public string[]? Fields { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
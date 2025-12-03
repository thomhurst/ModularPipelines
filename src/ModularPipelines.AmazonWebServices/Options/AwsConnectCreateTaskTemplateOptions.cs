using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-task-template")]
public record AwsConnectCreateTaskTemplateOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--fields")] string[] Fields
) : AwsOptions
{
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

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
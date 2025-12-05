using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-contact-flow")]
public record AwsConnectCreateContactFlowOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type,
[property: CliOption("--content")] string Content
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
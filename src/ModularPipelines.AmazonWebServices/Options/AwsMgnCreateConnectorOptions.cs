using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "create-connector")]
public record AwsMgnCreateConnectorOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--ssm-instance-id")] string SsmInstanceId
) : AwsOptions
{
    [CliOption("--ssm-command-config")]
    public string? SsmCommandConfig { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
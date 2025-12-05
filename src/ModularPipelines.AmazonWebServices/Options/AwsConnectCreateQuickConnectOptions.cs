using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-quick-connect")]
public record AwsConnectCreateQuickConnectOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--quick-connect-config")] string QuickConnectConfig
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
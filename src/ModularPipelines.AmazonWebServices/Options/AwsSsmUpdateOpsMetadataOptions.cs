using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "update-ops-metadata")]
public record AwsSsmUpdateOpsMetadataOptions(
[property: CommandSwitch("--ops-metadata-arn")] string OpsMetadataArn
) : AwsOptions
{
    [CommandSwitch("--metadata-to-update")]
    public IEnumerable<KeyValue>? MetadataToUpdate { get; set; }

    [CommandSwitch("--keys-to-delete")]
    public string[]? KeysToDelete { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
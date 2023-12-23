using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "start-variant-import-job")]
public record AwsOmicsStartVariantImportJobOptions(
[property: CommandSwitch("--destination-name")] string DestinationName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--items")] string[] Items
) : AwsOptions
{
    [CommandSwitch("--annotation-fields")]
    public IEnumerable<KeyValue>? AnnotationFields { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
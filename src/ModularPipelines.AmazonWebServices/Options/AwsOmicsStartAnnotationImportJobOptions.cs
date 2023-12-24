using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "start-annotation-import-job")]
public record AwsOmicsStartAnnotationImportJobOptions(
[property: CommandSwitch("--destination-name")] string DestinationName,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--items")] string[] Items
) : AwsOptions
{
    [CommandSwitch("--version-name")]
    public string? VersionName { get; set; }

    [CommandSwitch("--format-options")]
    public string? FormatOptions { get; set; }

    [CommandSwitch("--annotation-fields")]
    public IEnumerable<KeyValue>? AnnotationFields { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
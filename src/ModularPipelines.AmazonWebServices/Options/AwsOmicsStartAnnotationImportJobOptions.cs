using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "start-annotation-import-job")]
public record AwsOmicsStartAnnotationImportJobOptions(
[property: CliOption("--destination-name")] string DestinationName,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--items")] string[] Items
) : AwsOptions
{
    [CliOption("--version-name")]
    public string? VersionName { get; set; }

    [CliOption("--format-options")]
    public string? FormatOptions { get; set; }

    [CliOption("--annotation-fields")]
    public IEnumerable<KeyValue>? AnnotationFields { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "start-test-set-generation")]
public record AwsLexv2ModelsStartTestSetGenerationOptions(
[property: CliOption("--test-set-name")] string TestSetName,
[property: CliOption("--storage-location")] string StorageLocation,
[property: CliOption("--generation-data-source")] string GenerationDataSource,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--test-set-tags")]
    public IEnumerable<KeyValue>? TestSetTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
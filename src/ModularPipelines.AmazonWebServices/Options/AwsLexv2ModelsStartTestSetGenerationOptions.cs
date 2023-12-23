using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "start-test-set-generation")]
public record AwsLexv2ModelsStartTestSetGenerationOptions(
[property: CommandSwitch("--test-set-name")] string TestSetName,
[property: CommandSwitch("--storage-location")] string StorageLocation,
[property: CommandSwitch("--generation-data-source")] string GenerationDataSource,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--test-set-tags")]
    public IEnumerable<KeyValue>? TestSetTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
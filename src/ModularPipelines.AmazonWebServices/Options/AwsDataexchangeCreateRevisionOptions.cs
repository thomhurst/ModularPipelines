using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataexchange", "create-revision")]
public record AwsDataexchangeCreateRevisionOptions(
[property: CommandSwitch("--data-set-id")] string DataSetId
) : AwsOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
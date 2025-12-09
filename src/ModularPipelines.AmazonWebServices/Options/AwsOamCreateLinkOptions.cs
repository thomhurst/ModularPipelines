using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("oam", "create-link")]
public record AwsOamCreateLinkOptions(
[property: CliOption("--label-template")] string LabelTemplate,
[property: CliOption("--resource-types")] string[] ResourceTypes,
[property: CliOption("--sink-identifier")] string SinkIdentifier
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
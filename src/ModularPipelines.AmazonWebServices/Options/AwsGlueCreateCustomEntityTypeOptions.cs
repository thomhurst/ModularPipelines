using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-custom-entity-type")]
public record AwsGlueCreateCustomEntityTypeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--regex-string")] string RegexString
) : AwsOptions
{
    [CliOption("--context-words")]
    public string[]? ContextWords { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
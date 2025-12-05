using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "create-workflow")]
public record AwsOmicsCreateWorkflowOptions : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--engine")]
    public string? Engine { get; set; }

    [CliOption("--definition-zip")]
    public string? DefinitionZip { get; set; }

    [CliOption("--definition-uri")]
    public string? DefinitionUri { get; set; }

    [CliOption("--main")]
    public string? Main { get; set; }

    [CliOption("--parameter-template")]
    public IEnumerable<KeyValue>? ParameterTemplate { get; set; }

    [CliOption("--storage-capacity")]
    public int? StorageCapacity { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--request-id")]
    public string? RequestId { get; set; }

    [CliOption("--accelerators")]
    public string? Accelerators { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
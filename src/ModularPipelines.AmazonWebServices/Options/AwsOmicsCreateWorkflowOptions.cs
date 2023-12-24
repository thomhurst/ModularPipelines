using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "create-workflow")]
public record AwsOmicsCreateWorkflowOptions : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--engine")]
    public string? Engine { get; set; }

    [CommandSwitch("--definition-zip")]
    public string? DefinitionZip { get; set; }

    [CommandSwitch("--definition-uri")]
    public string? DefinitionUri { get; set; }

    [CommandSwitch("--main")]
    public string? Main { get; set; }

    [CommandSwitch("--parameter-template")]
    public IEnumerable<KeyValue>? ParameterTemplate { get; set; }

    [CommandSwitch("--storage-capacity")]
    public int? StorageCapacity { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--request-id")]
    public string? RequestId { get; set; }

    [CommandSwitch("--accelerators")]
    public string? Accelerators { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
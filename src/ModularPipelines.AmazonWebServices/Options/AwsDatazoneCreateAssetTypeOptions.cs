using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "create-asset-type")]
public record AwsDatazoneCreateAssetTypeOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--forms-input")] IEnumerable<KeyValue> FormsInput,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--owning-project-identifier")] string OwningProjectIdentifier
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
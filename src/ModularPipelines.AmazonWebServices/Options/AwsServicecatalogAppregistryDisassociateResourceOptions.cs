using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog-appregistry", "disassociate-resource")]
public record AwsServicecatalogAppregistryDisassociateResourceOptions(
[property: CommandSwitch("--application")] string Application,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--resource")] string Resource
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
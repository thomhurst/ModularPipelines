using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog-appregistry", "associate-resource")]
public record AwsServicecatalogAppregistryAssociateResourceOptions(
[property: CommandSwitch("--application")] string Application,
[property: CommandSwitch("--resource-type")] string ResourceType,
[property: CommandSwitch("--resource")] string Resource
) : AwsOptions
{
    [CommandSwitch("--options")]
    public string[]? Options { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
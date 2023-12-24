using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog-appregistry", "put-configuration")]
public record AwsServicecatalogAppregistryPutConfigurationOptions(
[property: CommandSwitch("--configuration")] string Configuration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
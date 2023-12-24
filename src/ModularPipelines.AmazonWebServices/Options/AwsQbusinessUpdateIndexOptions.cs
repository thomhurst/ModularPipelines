using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "update-index")]
public record AwsQbusinessUpdateIndexOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--index-id")] string IndexId
) : AwsOptions
{
    [CommandSwitch("--capacity-configuration")]
    public string? CapacityConfiguration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--document-attribute-configurations")]
    public string[]? DocumentAttributeConfigurations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "remove-template-action")]
public record AwsMgnRemoveTemplateActionOptions(
[property: CliOption("--action-id")] string ActionId,
[property: CliOption("--launch-configuration-template-id")] string LaunchConfigurationTemplateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
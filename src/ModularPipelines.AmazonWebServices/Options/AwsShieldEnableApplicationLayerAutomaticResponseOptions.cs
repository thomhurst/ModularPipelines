using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("shield", "enable-application-layer-automatic-response")]
public record AwsShieldEnableApplicationLayerAutomaticResponseOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--action")] string Action
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
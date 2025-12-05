using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "get-infrastructure-configuration")]
public record AwsImagebuilderGetInfrastructureConfigurationOptions(
[property: CliOption("--infrastructure-configuration-arn")] string InfrastructureConfigurationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
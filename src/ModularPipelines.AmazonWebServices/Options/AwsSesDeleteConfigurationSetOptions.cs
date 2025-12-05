using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ses", "delete-configuration-set")]
public record AwsSesDeleteConfigurationSetOptions(
[property: CliOption("--configuration-set-name")] string ConfigurationSetName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivschat", "update-logging-configuration")]
public record AwsIvschatUpdateLoggingConfigurationOptions(
[property: CliOption("--identifier")] string Identifier
) : AwsOptions
{
    [CliOption("--destination-configuration")]
    public string? DestinationConfiguration { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
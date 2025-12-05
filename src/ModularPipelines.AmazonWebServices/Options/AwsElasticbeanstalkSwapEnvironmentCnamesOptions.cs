using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "swap-environment-cnames")]
public record AwsElasticbeanstalkSwapEnvironmentCnamesOptions : AwsOptions
{
    [CliOption("--source-environment-id")]
    public string? SourceEnvironmentId { get; set; }

    [CliOption("--source-environment-name")]
    public string? SourceEnvironmentName { get; set; }

    [CliOption("--destination-environment-id")]
    public string? DestinationEnvironmentId { get; set; }

    [CliOption("--destination-environment-name")]
    public string? DestinationEnvironmentName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
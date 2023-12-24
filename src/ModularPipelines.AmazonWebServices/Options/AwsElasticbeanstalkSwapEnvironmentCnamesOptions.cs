using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "swap-environment-cnames")]
public record AwsElasticbeanstalkSwapEnvironmentCnamesOptions : AwsOptions
{
    [CommandSwitch("--source-environment-id")]
    public string? SourceEnvironmentId { get; set; }

    [CommandSwitch("--source-environment-name")]
    public string? SourceEnvironmentName { get; set; }

    [CommandSwitch("--destination-environment-id")]
    public string? DestinationEnvironmentId { get; set; }

    [CommandSwitch("--destination-environment-name")]
    public string? DestinationEnvironmentName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
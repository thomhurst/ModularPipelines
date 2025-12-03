using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "create-project")]
public record AwsDevicefarmCreateProjectOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--default-job-timeout-minutes")]
    public int? DefaultJobTimeoutMinutes { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
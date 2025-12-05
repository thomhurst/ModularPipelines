using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediastore", "put-cors-policy")]
public record AwsMediastorePutCorsPolicyOptions(
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--cors-policy")] string[] CorsPolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ecr", "get-authorization-token")]
public record AwsEcrGetAuthorizationTokenOptions : AwsOptions
{
    [CliOption("--registry-ids")]
    public string[]? RegistryIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
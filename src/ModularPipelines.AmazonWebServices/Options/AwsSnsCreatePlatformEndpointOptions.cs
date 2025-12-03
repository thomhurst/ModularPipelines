using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "create-platform-endpoint")]
public record AwsSnsCreatePlatformEndpointOptions(
[property: CliOption("--platform-application-arn")] string PlatformApplicationArn,
[property: CliOption("--token")] string Token
) : AwsOptions
{
    [CliOption("--custom-user-data")]
    public string? CustomUserData { get; set; }

    [CliOption("--attributes")]
    public IEnumerable<KeyValue>? Attributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
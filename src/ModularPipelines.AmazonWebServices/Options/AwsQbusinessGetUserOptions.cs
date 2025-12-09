using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "get-user")]
public record AwsQbusinessGetUserOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
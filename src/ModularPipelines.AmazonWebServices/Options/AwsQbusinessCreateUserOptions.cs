using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "create-user")]
public record AwsQbusinessCreateUserOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--user-aliases")]
    public string[]? UserAliases { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
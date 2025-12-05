using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "update-user")]
public record AwsQbusinessUpdateUserOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--user-aliases-to-delete")]
    public string[]? UserAliasesToDelete { get; set; }

    [CliOption("--user-aliases-to-update")]
    public string[]? UserAliasesToUpdate { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
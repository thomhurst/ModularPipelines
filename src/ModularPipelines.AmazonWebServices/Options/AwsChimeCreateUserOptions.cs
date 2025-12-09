using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "create-user")]
public record AwsChimeCreateUserOptions(
[property: CliOption("--account-id")] string AccountId
) : AwsOptions
{
    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--email")]
    public string? Email { get; set; }

    [CliOption("--user-type")]
    public string? UserType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
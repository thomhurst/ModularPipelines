using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "update-user")]
public record AwsWorkdocsUpdateUserOptions(
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--given-name")]
    public string? GivenName { get; set; }

    [CliOption("--surname")]
    public string? Surname { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--storage-rule")]
    public string? StorageRule { get; set; }

    [CliOption("--time-zone-id")]
    public string? TimeZoneId { get; set; }

    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--grant-poweruser-privileges")]
    public string? GrantPoweruserPrivileges { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
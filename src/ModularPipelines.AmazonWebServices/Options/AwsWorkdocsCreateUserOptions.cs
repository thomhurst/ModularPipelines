using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workdocs", "create-user")]
public record AwsWorkdocsCreateUserOptions(
[property: CliOption("--username")] string Username,
[property: CliOption("--given-name")] string GivenName,
[property: CliOption("--surname")] string Surname,
[property: CliOption("--password")] string Password
) : AwsOptions
{
    [CliOption("--organization-id")]
    public string? OrganizationId { get; set; }

    [CliOption("--email-address")]
    public string? EmailAddress { get; set; }

    [CliOption("--time-zone-id")]
    public string? TimeZoneId { get; set; }

    [CliOption("--storage-rule")]
    public string? StorageRule { get; set; }

    [CliOption("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
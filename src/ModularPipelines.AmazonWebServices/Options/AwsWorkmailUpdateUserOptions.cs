using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "update-user")]
public record AwsWorkmailUpdateUserOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--first-name")]
    public string? FirstName { get; set; }

    [CliOption("--last-name")]
    public string? LastName { get; set; }

    [CliOption("--initials")]
    public string? Initials { get; set; }

    [CliOption("--telephone")]
    public string? Telephone { get; set; }

    [CliOption("--street")]
    public string? Street { get; set; }

    [CliOption("--job-title")]
    public string? JobTitle { get; set; }

    [CliOption("--city")]
    public string? City { get; set; }

    [CliOption("--company")]
    public string? Company { get; set; }

    [CliOption("--zip-code")]
    public string? ZipCode { get; set; }

    [CliOption("--department")]
    public string? Department { get; set; }

    [CliOption("--country")]
    public string? Country { get; set; }

    [CliOption("--office")]
    public string? Office { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
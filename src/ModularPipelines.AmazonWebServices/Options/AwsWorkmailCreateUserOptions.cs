using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "create-user")]
public record AwsWorkmailCreateUserOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--name")] string Name,
[property: CliOption("--display-name")] string DisplayName
) : AwsOptions
{
    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--first-name")]
    public string? FirstName { get; set; }

    [CliOption("--last-name")]
    public string? LastName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
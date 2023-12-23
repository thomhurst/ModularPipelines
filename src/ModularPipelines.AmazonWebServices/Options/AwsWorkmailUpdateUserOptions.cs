using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "update-user")]
public record AwsWorkmailUpdateUserOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--first-name")]
    public string? FirstName { get; set; }

    [CommandSwitch("--last-name")]
    public string? LastName { get; set; }

    [CommandSwitch("--initials")]
    public string? Initials { get; set; }

    [CommandSwitch("--telephone")]
    public string? Telephone { get; set; }

    [CommandSwitch("--street")]
    public string? Street { get; set; }

    [CommandSwitch("--job-title")]
    public string? JobTitle { get; set; }

    [CommandSwitch("--city")]
    public string? City { get; set; }

    [CommandSwitch("--company")]
    public string? Company { get; set; }

    [CommandSwitch("--zip-code")]
    public string? ZipCode { get; set; }

    [CommandSwitch("--department")]
    public string? Department { get; set; }

    [CommandSwitch("--country")]
    public string? Country { get; set; }

    [CommandSwitch("--office")]
    public string? Office { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
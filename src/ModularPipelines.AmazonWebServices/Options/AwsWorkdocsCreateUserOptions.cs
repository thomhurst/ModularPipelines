using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "create-user")]
public record AwsWorkdocsCreateUserOptions(
[property: CommandSwitch("--username")] string Username,
[property: CommandSwitch("--given-name")] string GivenName,
[property: CommandSwitch("--surname")] string Surname,
[property: CommandSwitch("--password")] string Password
) : AwsOptions
{
    [CommandSwitch("--organization-id")]
    public string? OrganizationId { get; set; }

    [CommandSwitch("--email-address")]
    public string? EmailAddress { get; set; }

    [CommandSwitch("--time-zone-id")]
    public string? TimeZoneId { get; set; }

    [CommandSwitch("--storage-rule")]
    public string? StorageRule { get; set; }

    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
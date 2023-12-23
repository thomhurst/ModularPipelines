using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "update-user")]
public record AwsWorkdocsUpdateUserOptions(
[property: CommandSwitch("--user-id")] string UserId
) : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--given-name")]
    public string? GivenName { get; set; }

    [CommandSwitch("--surname")]
    public string? Surname { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--storage-rule")]
    public string? StorageRule { get; set; }

    [CommandSwitch("--time-zone-id")]
    public string? TimeZoneId { get; set; }

    [CommandSwitch("--locale")]
    public string? Locale { get; set; }

    [CommandSwitch("--grant-poweruser-privileges")]
    public string? GrantPoweruserPrivileges { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
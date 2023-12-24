using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rolesanywhere", "update-profile")]
public record AwsRolesanywhereUpdateProfileOptions(
[property: CommandSwitch("--profile-id")] string ProfileId
) : AwsOptions
{
    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CommandSwitch("--managed-policy-arns")]
    public string[]? ManagedPolicyArns { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--role-arns")]
    public string[]? RoleArns { get; set; }

    [CommandSwitch("--session-policy")]
    public string? SessionPolicy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
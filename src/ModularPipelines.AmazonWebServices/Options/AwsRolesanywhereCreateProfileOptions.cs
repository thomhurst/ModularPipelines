using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rolesanywhere", "create-profile")]
public record AwsRolesanywhereCreateProfileOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--role-arns")] string[] RoleArns
) : AwsOptions
{
    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CommandSwitch("--managed-policy-arns")]
    public string[]? ManagedPolicyArns { get; set; }

    [CommandSwitch("--session-policy")]
    public string? SessionPolicy { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
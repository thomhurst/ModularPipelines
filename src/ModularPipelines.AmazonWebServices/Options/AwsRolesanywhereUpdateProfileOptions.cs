using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rolesanywhere", "update-profile")]
public record AwsRolesanywhereUpdateProfileOptions(
[property: CliOption("--profile-id")] string ProfileId
) : AwsOptions
{
    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--managed-policy-arns")]
    public string[]? ManagedPolicyArns { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--role-arns")]
    public string[]? RoleArns { get; set; }

    [CliOption("--session-policy")]
    public string? SessionPolicy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
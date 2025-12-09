using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rolesanywhere", "create-profile")]
public record AwsRolesanywhereCreateProfileOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--role-arns")] string[] RoleArns
) : AwsOptions
{
    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--managed-policy-arns")]
    public string[]? ManagedPolicyArns { get; set; }

    [CliOption("--session-policy")]
    public string? SessionPolicy { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
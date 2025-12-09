using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-db-security-group")]
public record AwsRdsCreateDbSecurityGroupOptions(
[property: CliOption("--db-security-group-name")] string DbSecurityGroupName,
[property: CliOption("--db-security-group-description")] string DbSecurityGroupDescription
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
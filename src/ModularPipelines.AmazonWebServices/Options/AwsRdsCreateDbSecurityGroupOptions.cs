using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "create-db-security-group")]
public record AwsRdsCreateDbSecurityGroupOptions(
[property: CommandSwitch("--db-security-group-name")] string DbSecurityGroupName,
[property: CommandSwitch("--db-security-group-description")] string DbSecurityGroupDescription
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
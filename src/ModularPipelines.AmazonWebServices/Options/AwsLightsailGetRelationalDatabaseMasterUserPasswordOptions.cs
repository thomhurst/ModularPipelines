using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-relational-database-master-user-password")]
public record AwsLightsailGetRelationalDatabaseMasterUserPasswordOptions(
[property: CommandSwitch("--relational-database-name")] string RelationalDatabaseName
) : AwsOptions
{
    [CommandSwitch("--password-version")]
    public string? PasswordVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
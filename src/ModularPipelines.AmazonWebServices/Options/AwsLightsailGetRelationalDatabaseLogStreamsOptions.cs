using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-relational-database-log-streams")]
public record AwsLightsailGetRelationalDatabaseLogStreamsOptions(
[property: CommandSwitch("--relational-database-name")] string RelationalDatabaseName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
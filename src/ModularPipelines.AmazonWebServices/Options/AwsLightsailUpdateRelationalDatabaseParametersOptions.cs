using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "update-relational-database-parameters")]
public record AwsLightsailUpdateRelationalDatabaseParametersOptions(
[property: CommandSwitch("--relational-database-name")] string RelationalDatabaseName,
[property: CommandSwitch("--parameters")] string[] Parameters
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
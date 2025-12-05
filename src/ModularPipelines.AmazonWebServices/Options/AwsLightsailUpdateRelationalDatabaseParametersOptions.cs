using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lightsail", "update-relational-database-parameters")]
public record AwsLightsailUpdateRelationalDatabaseParametersOptions(
[property: CliOption("--relational-database-name")] string RelationalDatabaseName,
[property: CliOption("--parameters")] string[] Parameters
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
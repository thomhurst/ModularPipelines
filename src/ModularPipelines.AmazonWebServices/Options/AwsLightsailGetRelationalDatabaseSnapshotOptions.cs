using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "get-relational-database-snapshot")]
public record AwsLightsailGetRelationalDatabaseSnapshotOptions(
[property: CommandSwitch("--relational-database-snapshot-name")] string RelationalDatabaseSnapshotName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
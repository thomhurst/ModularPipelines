using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "delete-relational-database-snapshot")]
public record AwsLightsailDeleteRelationalDatabaseSnapshotOptions(
[property: CommandSwitch("--relational-database-snapshot-name")] string RelationalDatabaseSnapshotName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
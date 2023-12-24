using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("timestream-write", "update-database")]
public record AwsTimestreamWriteUpdateDatabaseOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--kms-key-id")] string KmsKeyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
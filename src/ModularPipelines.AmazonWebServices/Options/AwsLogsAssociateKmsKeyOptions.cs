using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "associate-kms-key")]
public record AwsLogsAssociateKmsKeyOptions(
[property: CommandSwitch("--kms-key-id")] string KmsKeyId
) : AwsOptions
{
    [CommandSwitch("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CommandSwitch("--resource-identifier")]
    public string? ResourceIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "disassociate-kms-key")]
public record AwsLogsDisassociateKmsKeyOptions : AwsOptions
{
    [CommandSwitch("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CommandSwitch("--resource-identifier")]
    public string? ResourceIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
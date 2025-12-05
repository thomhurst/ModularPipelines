using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "disassociate-kms-key")]
public record AwsLogsDisassociateKmsKeyOptions : AwsOptions
{
    [CliOption("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CliOption("--resource-identifier")]
    public string? ResourceIdentifier { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
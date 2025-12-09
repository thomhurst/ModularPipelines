using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-quick-connect-config")]
public record AwsConnectUpdateQuickConnectConfigOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--quick-connect-id")] string QuickConnectId,
[property: CliOption("--quick-connect-config")] string QuickConnectConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
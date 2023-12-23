using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudhsmv2", "delete-hsm")]
public record AwsCloudhsmv2DeleteHsmOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId
) : AwsOptions
{
    [CommandSwitch("--hsm-id")]
    public string? HsmId { get; set; }

    [CommandSwitch("--eni-id")]
    public string? EniId { get; set; }

    [CommandSwitch("--eni-ip")]
    public string? EniIp { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
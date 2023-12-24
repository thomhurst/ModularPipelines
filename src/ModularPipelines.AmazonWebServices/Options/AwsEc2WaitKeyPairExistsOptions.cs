using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "wait", "key-pair-exists")]
public record AwsEc2WaitKeyPairExistsOptions : AwsOptions
{
    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--key-names")]
    public string[]? KeyNames { get; set; }

    [CommandSwitch("--key-pair-ids")]
    public string[]? KeyPairIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "associate-mac-sec-key")]
public record AwsDirectconnectAssociateMacSecKeyOptions(
[property: CommandSwitch("--connection-id")] string ConnectionId
) : AwsOptions
{
    [CommandSwitch("--secret-arn")]
    public string? SecretArn { get; set; }

    [CommandSwitch("--ckn")]
    public string? Ckn { get; set; }

    [CommandSwitch("--cak")]
    public string? Cak { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
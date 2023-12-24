using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("waf-regional", "delete-byte-match-set")]
public record AwsWafRegionalDeleteByteMatchSetOptions(
[property: CommandSwitch("--byte-match-set-id")] string ByteMatchSetId,
[property: CommandSwitch("--change-token")] string ChangeToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
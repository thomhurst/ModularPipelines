using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sts", "decode-authorization-message")]
public record AwsStsDecodeAuthorizationMessageOptions(
[property: CommandSwitch("--encoded-message")] string EncodedMessage
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
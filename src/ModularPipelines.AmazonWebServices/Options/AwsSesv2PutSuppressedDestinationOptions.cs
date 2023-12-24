using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "put-suppressed-destination")]
public record AwsSesv2PutSuppressedDestinationOptions(
[property: CommandSwitch("--email-address")] string EmailAddress,
[property: CommandSwitch("--reason")] string Reason
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "put-email-identity-dkim-signing-attributes")]
public record AwsSesv2PutEmailIdentityDkimSigningAttributesOptions(
[property: CommandSwitch("--email-identity")] string EmailIdentity,
[property: CommandSwitch("--signing-attributes-origin")] string SigningAttributesOrigin,
[property: CommandSwitch("--signing-attributes")] string SigningAttributes
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
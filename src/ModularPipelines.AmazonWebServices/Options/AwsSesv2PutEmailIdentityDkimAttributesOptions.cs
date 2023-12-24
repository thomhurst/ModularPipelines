using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sesv2", "put-email-identity-dkim-attributes")]
public record AwsSesv2PutEmailIdentityDkimAttributesOptions(
[property: CommandSwitch("--email-identity")] string EmailIdentity
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
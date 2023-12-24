using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ses", "get-identity-mail-from-domain-attributes")]
public record AwsSesGetIdentityMailFromDomainAttributesOptions(
[property: CommandSwitch("--identities")] string[] Identities
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
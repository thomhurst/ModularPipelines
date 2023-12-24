using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "get-iam-portal-login-url")]
public record AwsDatazoneGetIamPortalLoginUrlOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
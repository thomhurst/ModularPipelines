using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "create-domain-name")]
public record AwsAppsyncCreateDomainNameOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--certificate-arn")] string CertificateArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
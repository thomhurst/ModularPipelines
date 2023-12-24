using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "put-inbound-dmarc-settings")]
public record AwsWorkmailPutInboundDmarcSettingsOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
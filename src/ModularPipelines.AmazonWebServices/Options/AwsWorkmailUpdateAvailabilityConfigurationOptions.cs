using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "update-availability-configuration")]
public record AwsWorkmailUpdateAvailabilityConfigurationOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--ews-provider")]
    public string? EwsProvider { get; set; }

    [CommandSwitch("--lambda-provider")]
    public string? LambdaProvider { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
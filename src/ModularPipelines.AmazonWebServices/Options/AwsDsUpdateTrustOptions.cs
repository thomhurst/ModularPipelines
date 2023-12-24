using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "update-trust")]
public record AwsDsUpdateTrustOptions(
[property: CommandSwitch("--trust-id")] string TrustId
) : AwsOptions
{
    [CommandSwitch("--selective-auth")]
    public string? SelectiveAuth { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
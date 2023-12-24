using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "create-audit-suppression")]
public record AwsIotCreateAuditSuppressionOptions(
[property: CommandSwitch("--check-name")] string CheckName,
[property: CommandSwitch("--resource-identifier")] string ResourceIdentifier
) : AwsOptions
{
    [CommandSwitch("--expiration-date")]
    public long? ExpirationDate { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
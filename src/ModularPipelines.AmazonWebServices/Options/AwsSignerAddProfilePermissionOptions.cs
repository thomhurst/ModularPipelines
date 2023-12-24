using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signer", "add-profile-permission")]
public record AwsSignerAddProfilePermissionOptions(
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--principal")] string Principal,
[property: CommandSwitch("--statement-id")] string StatementId
) : AwsOptions
{
    [CommandSwitch("--profile-version")]
    public string? ProfileVersion { get; set; }

    [CommandSwitch("--revision-id")]
    public string? RevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("signer", "start-signing-job")]
public record AwsSignerStartSigningJobOptions(
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--profile-name")] string ProfileName
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--profile-owner")]
    public string? ProfileOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
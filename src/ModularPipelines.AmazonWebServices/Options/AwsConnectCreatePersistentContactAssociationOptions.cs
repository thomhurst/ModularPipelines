using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-persistent-contact-association")]
public record AwsConnectCreatePersistentContactAssociationOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--initial-contact-id")] string InitialContactId,
[property: CommandSwitch("--rehydration-type")] string RehydrationType,
[property: CommandSwitch("--source-contact-id")] string SourceContactId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
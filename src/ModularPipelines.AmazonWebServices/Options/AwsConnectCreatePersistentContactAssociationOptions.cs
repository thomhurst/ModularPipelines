using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "create-persistent-contact-association")]
public record AwsConnectCreatePersistentContactAssociationOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--initial-contact-id")] string InitialContactId,
[property: CliOption("--rehydration-type")] string RehydrationType,
[property: CliOption("--source-contact-id")] string SourceContactId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
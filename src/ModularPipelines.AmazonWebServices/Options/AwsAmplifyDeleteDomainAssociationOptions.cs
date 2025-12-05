using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplify", "delete-domain-association")]
public record AwsAmplifyDeleteDomainAssociationOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
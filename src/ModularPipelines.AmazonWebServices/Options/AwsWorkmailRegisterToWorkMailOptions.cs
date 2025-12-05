using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "register-to-work-mail")]
public record AwsWorkmailRegisterToWorkMailOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--entity-id")] string EntityId,
[property: CliOption("--email")] string Email
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
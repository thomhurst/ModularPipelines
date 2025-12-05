using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53domains", "resend-operation-authorization")]
public record AwsRoute53domainsResendOperationAuthorizationOptions(
[property: CliOption("--operation-id")] string OperationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
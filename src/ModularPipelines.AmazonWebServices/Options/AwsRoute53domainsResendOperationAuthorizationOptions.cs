using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53domains", "resend-operation-authorization")]
public record AwsRoute53domainsResendOperationAuthorizationOptions(
[property: CommandSwitch("--operation-id")] string OperationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
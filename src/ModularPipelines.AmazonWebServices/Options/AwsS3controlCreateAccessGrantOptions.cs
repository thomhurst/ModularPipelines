using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "create-access-grant")]
public record AwsS3controlCreateAccessGrantOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--access-grants-location-id")] string AccessGrantsLocationId,
[property: CommandSwitch("--grantee")] string Grantee,
[property: CommandSwitch("--permission")] string Permission
) : AwsOptions
{
    [CommandSwitch("--access-grants-location-configuration")]
    public string? AccessGrantsLocationConfiguration { get; set; }

    [CommandSwitch("--application-arn")]
    public string? ApplicationArn { get; set; }

    [CommandSwitch("--s3-prefix-type")]
    public string? S3PrefixType { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
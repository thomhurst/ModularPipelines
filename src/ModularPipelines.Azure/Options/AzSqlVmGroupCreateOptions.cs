using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "vm", "group", "create")]
public record AzSqlVmGroupCreateOptions(
[property: CliOption("--domain-fqdn")] string DomainFqdn,
[property: CliOption("--image-offer")] string ImageOffer,
[property: CliOption("--image-sku")] string ImageSku,
[property: CliOption("--name")] string Name,
[property: CliOption("--operator-acc")] string OperatorAcc,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-acc")] string ServiceAcc,
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions
{
    [CliOption("--bootstrap-acc")]
    public string? BootstrapAcc { get; set; }

    [CliOption("--cluster-subnet-type")]
    public string? ClusterSubnetType { get; set; }

    [CliOption("--fsw-path")]
    public string? FswPath { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--ou-path")]
    public string? OuPath { get; set; }

    [CliOption("--sa-key")]
    public string? SaKey { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "vm", "group", "create")]
public record AzSqlVmGroupCreateOptions(
[property: CommandSwitch("--domain-fqdn")] string DomainFqdn,
[property: CommandSwitch("--image-offer")] string ImageOffer,
[property: CommandSwitch("--image-sku")] string ImageSku,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--operator-acc")] string OperatorAcc,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-acc")] string ServiceAcc,
[property: CommandSwitch("--storage-account")] int StorageAccount
) : AzOptions
{
    [CommandSwitch("--bootstrap-acc")]
    public string? BootstrapAcc { get; set; }

    [CommandSwitch("--cluster-subnet-type")]
    public string? ClusterSubnetType { get; set; }

    [CommandSwitch("--fsw-path")]
    public string? FswPath { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--ou-path")]
    public string? OuPath { get; set; }

    [CommandSwitch("--sa-key")]
    public string? SaKey { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}
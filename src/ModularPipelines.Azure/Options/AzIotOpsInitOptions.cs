using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "init")]
public record AzIotOpsInitOptions(
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ca-dir")]
    public string? CaDir { get; set; }

    [CommandSwitch("--ca-file")]
    public string? CaFile { get; set; }

    [CommandSwitch("--ca-key-file")]
    public string? CaKeyFile { get; set; }

    [CommandSwitch("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CommandSwitch("--cluster-namespace")]
    public string? ClusterNamespace { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--custom-location")]
    public string? CustomLocation { get; set; }

    [BooleanCommandSwitch("--disable-rotation")]
    public bool? DisableRotation { get; set; }

    [CommandSwitch("--dp-instance")]
    public string? DpInstance { get; set; }

    [CommandSwitch("--dp-message-stores")]
    public string? DpMessageStores { get; set; }

    [CommandSwitch("--dp-reader-workers")]
    public string? DpReaderWorkers { get; set; }

    [CommandSwitch("--dp-runner-workers")]
    public string? DpRunnerWorkers { get; set; }

    [CommandSwitch("--kv-id")]
    public string? KvId { get; set; }

    [CommandSwitch("--kv-sat-secret-name")]
    public string? KvSatSecretName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--mq-authn")]
    public string? MqAuthn { get; set; }

    [CommandSwitch("--mq-backend-part")]
    public string? MqBackendPart { get; set; }

    [CommandSwitch("--mq-backend-rf")]
    public string? MqBackendRf { get; set; }

    [CommandSwitch("--mq-backend-workers")]
    public string? MqBackendWorkers { get; set; }

    [CommandSwitch("--mq-broker")]
    public string? MqBroker { get; set; }

    [CommandSwitch("--mq-frontend-replicas")]
    public string? MqFrontendReplicas { get; set; }

    [CommandSwitch("--mq-frontend-server")]
    public string? MqFrontendServer { get; set; }

    [CommandSwitch("--mq-frontend-workers")]
    public string? MqFrontendWorkers { get; set; }

    [BooleanCommandSwitch("--mq-insecure")]
    public bool? MqInsecure { get; set; }

    [CommandSwitch("--mq-instance")]
    public string? MqInstance { get; set; }

    [CommandSwitch("--mq-listener")]
    public string? MqListener { get; set; }

    [CommandSwitch("--mq-mem-profile")]
    public string? MqMemProfile { get; set; }

    [CommandSwitch("--mq-mode")]
    public string? MqMode { get; set; }

    [CommandSwitch("--mq-service-type")]
    public string? MqServiceType { get; set; }

    [BooleanCommandSwitch("--no-block")]
    public bool? NoBlock { get; set; }

    [BooleanCommandSwitch("--no-deploy")]
    public bool? NoDeploy { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--no-tls")]
    public bool? NoTls { get; set; }

    [CommandSwitch("--opcua-discovery-url")]
    public string? OpcuaDiscoveryUrl { get; set; }

    [CommandSwitch("--rotation-int")]
    public string? RotationInt { get; set; }

    [BooleanCommandSwitch("--show-template")]
    public bool? ShowTemplate { get; set; }

    [BooleanCommandSwitch("--simulate-plc")]
    public bool? SimulatePlc { get; set; }

    [CommandSwitch("--sp-app-id")]
    public string? SpAppId { get; set; }

    [CommandSwitch("--sp-object-id")]
    public string? SpObjectId { get; set; }

    [CommandSwitch("--sp-secret")]
    public string? SpSecret { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }
}


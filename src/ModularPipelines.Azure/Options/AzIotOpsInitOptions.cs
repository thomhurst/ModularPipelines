using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "ops", "init")]
public record AzIotOpsInitOptions(
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--ca-dir")]
    public string? CaDir { get; set; }

    [CliOption("--ca-file")]
    public string? CaFile { get; set; }

    [CliOption("--ca-key-file")]
    public string? CaKeyFile { get; set; }

    [CliOption("--cluster-location")]
    public string? ClusterLocation { get; set; }

    [CliOption("--cluster-namespace")]
    public string? ClusterNamespace { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--custom-location")]
    public string? CustomLocation { get; set; }

    [CliFlag("--disable-rotation")]
    public bool? DisableRotation { get; set; }

    [CliOption("--dp-instance")]
    public string? DpInstance { get; set; }

    [CliOption("--dp-message-stores")]
    public string? DpMessageStores { get; set; }

    [CliOption("--dp-reader-workers")]
    public string? DpReaderWorkers { get; set; }

    [CliOption("--dp-runner-workers")]
    public string? DpRunnerWorkers { get; set; }

    [CliOption("--kv-id")]
    public string? KvId { get; set; }

    [CliOption("--kv-sat-secret-name")]
    public string? KvSatSecretName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mq-authn")]
    public string? MqAuthn { get; set; }

    [CliOption("--mq-backend-part")]
    public string? MqBackendPart { get; set; }

    [CliOption("--mq-backend-rf")]
    public string? MqBackendRf { get; set; }

    [CliOption("--mq-backend-workers")]
    public string? MqBackendWorkers { get; set; }

    [CliOption("--mq-broker")]
    public string? MqBroker { get; set; }

    [CliOption("--mq-frontend-replicas")]
    public string? MqFrontendReplicas { get; set; }

    [CliOption("--mq-frontend-server")]
    public string? MqFrontendServer { get; set; }

    [CliOption("--mq-frontend-workers")]
    public string? MqFrontendWorkers { get; set; }

    [CliFlag("--mq-insecure")]
    public bool? MqInsecure { get; set; }

    [CliOption("--mq-instance")]
    public string? MqInstance { get; set; }

    [CliOption("--mq-listener")]
    public string? MqListener { get; set; }

    [CliOption("--mq-mem-profile")]
    public string? MqMemProfile { get; set; }

    [CliOption("--mq-mode")]
    public string? MqMode { get; set; }

    [CliOption("--mq-service-type")]
    public string? MqServiceType { get; set; }

    [CliFlag("--no-block")]
    public bool? NoBlock { get; set; }

    [CliFlag("--no-deploy")]
    public bool? NoDeploy { get; set; }

    [CliFlag("--no-progress")]
    public bool? NoProgress { get; set; }

    [CliFlag("--no-tls")]
    public bool? NoTls { get; set; }

    [CliOption("--opcua-discovery-url")]
    public string? OpcuaDiscoveryUrl { get; set; }

    [CliOption("--rotation-int")]
    public string? RotationInt { get; set; }

    [CliFlag("--show-template")]
    public bool? ShowTemplate { get; set; }

    [CliFlag("--simulate-plc")]
    public bool? SimulatePlc { get; set; }

    [CliOption("--sp-app-id")]
    public string? SpAppId { get; set; }

    [CliOption("--sp-object-id")]
    public string? SpObjectId { get; set; }

    [CliOption("--sp-secret")]
    public string? SpSecret { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }
}
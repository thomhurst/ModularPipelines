using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public class AzMesh
{
    public AzMesh(
        AzMeshApp app,
        AzMeshCodePackageLog codePackageLog,
        AzMeshDeployment deployment,
        AzMeshGateway gateway,
        AzMeshGenerate generate,
        AzMeshNetwork network,
        AzMeshSecret secret,
        AzMeshSecretvalue secretvalue,
        AzMeshService service,
        AzMeshServiceReplica serviceReplica,
        AzMeshVolume volume
    )
    {
        App = app;
        CodePackageLog = codePackageLog;
        Deployment = deployment;
        Gateway = gateway;
        Generate = generate;
        Network = network;
        Secret = secret;
        Secretvalue = secretvalue;
        Service = service;
        ServiceReplica = serviceReplica;
        Volume = volume;
    }

    public AzMeshApp App { get; }

    public AzMeshCodePackageLog CodePackageLog { get; }

    public AzMeshDeployment Deployment { get; }

    public AzMeshGateway Gateway { get; }

    public AzMeshGenerate Generate { get; }

    public AzMeshNetwork Network { get; }

    public AzMeshSecret Secret { get; }

    public AzMeshSecretvalue Secretvalue { get; }

    public AzMeshService Service { get; }

    public AzMeshServiceReplica ServiceReplica { get; }

    public AzMeshVolume Volume { get; }
}


# Eksctl Package

`ModularPipelines.Eksctl` provides strongly typed access to the eksctl CLI for creating and managing Amazon EKS clusters and their resources.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Eksctl
```

The `eksctl` executable must be installed and available on `PATH` when the pipeline runs.

## Create a cluster[​](#create-a-cluster "Direct link to Create a cluster")

```
using ModularPipelines.Eksctl.Options;



var result = await context.Tools.Eksctl.Create.ClusterAsync(

    new EksctlCreateClusterOptions

    {

        Name = "production",

        Region = "eu-west-2",

        Zones = ["eu-west-2a", "eu-west-2b"],

        Nodes = 3,

        Managed = true,

    },

    cancellationToken: cancellationToken);
```

This renders the equivalent of:

```
eksctl create cluster --name=production --region=eu-west-2 --zones=eu-west-2a --zones=eu-west-2b --nodes=3 --managed
```

## Update cluster logging[​](#update-cluster-logging "Direct link to Update cluster logging")

```
using ModularPipelines.Eksctl.Enums;



var result = await context.Tools.Eksctl.Utils.UpdateClusterLoggingAsync(

    new EksctlUtilsUpdateClusterLoggingOptions

    {

        Cluster = "production",

        Approve = true,

        EnableTypes =

        [

            EksctlUtilsUpdateClusterLoggingEnableTypes.Api,

            EksctlUtilsUpdateClusterLoggingEnableTypes.Audit,

        ],

    },

    cancellationToken: cancellationToken);
```

Eksctl obtains AWS credentials from the standard AWS credential chain. The generated API does not expose access-key, secret-key, session-token, or password command-line options.

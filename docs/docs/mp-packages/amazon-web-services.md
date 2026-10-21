---
title: Amazon Web Services Package
---

# Amazon Web Services Package

Strongly typed AWS CLI commands and AWS SDK helpers for S3.

## Installation

```shell
dotnet add package ModularPipelines.AmazonWebServices
```

Required command-line tool: `aws`. It must be installed and available on `PATH` when invoking CLI commands.

## AWS CLI

Use `context.Tools.Aws` inside a module to access the generated CLI services, including
`context.Tools.Aws.Lambda`. See the [AWS CLI reference](cli/aws.md) for available commands
and options. Projects without the generated C# tool properties can use
`context.Tools.Get<ModularPipelines.AmazonWebServices.Services.IAws>()`.

## AWS SDK helpers

Register a caller-owned `AmazonS3Client` with `builder.Services.AddS3Client(client)`
from `ModularPipelines.AmazonWebServices.Extensions`. This registers scoped `IAmazon`,
`IAmazonProvisioner`, and `IS3` services, plus the AWS CLI services. Access S3 through
`context.Amazon().Provisioner.S3`; the caller remains responsible for disposing the client.

The handwritten `ModularPipelines.AmazonWebServices.Lambda` SDK wrapper was removed in V4.
It only forwarded calls to `AmazonLambdaClient` and had no integration context entry point.
For SDK operations, reference `AWSSDK.Lambda` directly and call the client's `*Async` methods
with the request and cancellation token. Your application owns the client configuration,
registration, and disposal. The generated Lambda CLI service remains available separately.

## Global options generation

The AWS CLI scraper reads the root `GLOBAL OPTIONS` section, including integer timeouts
and enum choices. It preserves command-specific options whose names also appear in that
section and excludes the version action from inherited settings.

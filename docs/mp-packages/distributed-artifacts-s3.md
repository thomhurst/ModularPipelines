# Distributed S3 Artifacts Package

`ModularPipelines.Distributed.Artifacts.S3` stores distributed pipeline artifacts in AWS S3 or an S3-compatible service such as Cloudflare R2, Backblaze B2, or MinIO.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Distributed.Artifacts.S3
```

## Configuration[​](#configuration "Direct link to Configuration")

Register the artifact store after enabling distributed mode:

```
using ModularPipelines.Distributed.Artifacts.S3.Extensions;

using ModularPipelines.Distributed.Extensions;



var builder = Pipeline.CreateBuilder(args);



builder.AddDistributedMode(options => options.TotalInstances = 2);

builder.AddS3DistributedArtifactStore(options =>

{

    options.BucketName = "pipeline-artifacts";

    options.Region = "eu-west-2";

});
```

Credentials use the AWS SDK credential chain. Configure the optional service URL when targeting an S3-compatible provider.

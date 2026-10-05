---
title: Amazon Web Services Package
---

# Amazon Web Services Package

Strongly typed AWS CLI commands and AWS SDK helpers.

## Installation

```shell
dotnet add package ModularPipelines.AmazonWebServices
```

Required command-line tool: `aws`. It must be installed and available on `PATH` when the pipeline runs.

## Global CLI options

Resolve the CLI service with `context.Tools.Aws`, then choose a service and command.
Every generated command options class inherits settings such as `Region`, `Profile`,
`Output`, and `NoCliPager` from `AwsOptions`:

```csharp
using ModularPipelines.AmazonWebServices.Enums;
using ModularPipelines.AmazonWebServices.Options;

var result = await context.Tools.Aws.Ec2.DescribeInstancesAsync(
    new AwsEc2DescribeInstancesOptions
    {
        Region = "eu-west-1",
        Profile = "build",
        Output = AwsOutput.Json,
        NoCliPager = true,
        InstanceIds = ["i-example"],
    },
    cancellationToken: cancellationToken);
```

Global settings render before the service and command; command parameters follow them.
The globals were generated from AWS CLI 2.37.9 help. `CliConnectTimeout` and `CliReadTimeout`
are integer seconds. `Output`, `Color`, `CliBinaryFormat`, and `CliErrorFormat` use generated
enums that retain the CLI value spelling. Boolean switches such as `NoCliPager` do not take
an extra value. Help and version actions are not inherited configuration settings.

These options select settings for one invocation. Credentials still come from the AWS
CLI's credential providers; `Profile` selects a configured profile. See the official
[AWS CLI global options reference](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-options.html).

## Migrating callers

Replace manually supplied global arguments with the inherited properties. Avoid supplying
the same setting both ways. Command-specific parameters remain on their command options
class, even when a global switch has the same spelling. The generator gives colliding
command properties a service prefix so neither property hides the other.

See the [generated AWS command reference](cli/aws.md) for the available commands.

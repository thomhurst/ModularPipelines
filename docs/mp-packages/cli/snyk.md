# snyk CLI reference

`ModularPipelines.Snyk` provides strongly typed access to the `snyk` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Snyk
```

Import `ModularPipelines.Snyk.Extensions`, then resolve the service with `context.Snyk()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Snyk.Extensions;

using ModularPipelines.Snyk.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Snyk().Auth(

            new SnykAuthOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                      | Options record                      |
| -------------------------------- | ----------------------------------- |
| `snyk aibom`                     | `SnykAibomOptions`                  |
| `snyk aibom test`                | `SnykAibomTestOptions`              |
| `snyk auth`                      | `SnykAuthOptions`                   |
| `snyk code test`                 | `SnykCodeTestOptions`               |
| `snyk container monitor`         | `SnykContainerMonitorOptions`       |
| `snyk container sbom`            | `SnykContainerSbomOptions`          |
| `snyk container test`            | `SnykContainerTestOptions`          |
| `snyk iac`                       | `SnykIacOptions`                    |
| `snyk iac describe`              | `SnykIacDescribeOptions`            |
| `snyk iac test`                  | `SnykIacTestOptions`                |
| `snyk iac update-exclude-policy` | `SnykIacUpdateExcludePolicyOptions` |
| `snyk ignore`                    | `SnykIgnoreOptions`                 |
| `snyk log4shell`                 | `SnykLog4shellOptions`              |
| `snyk monitor`                   | `SnykMonitorOptions`                |
| `snyk policy`                    | `SnykPolicyOptions`                 |
| `snyk sbom`                      | `SnykSbomOptions`                   |
| `snyk sbom test`                 | `SnykSbomTestOptions`               |
| `snyk test`                      | `SnykTestOptions`                   |

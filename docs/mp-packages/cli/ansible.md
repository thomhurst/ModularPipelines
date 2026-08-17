# ansible CLI reference

`ModularPipelines.Ansible` provides strongly typed access to the `ansible` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Ansible
```

Import `ModularPipelines.Ansible.Extensions`, then resolve the service with `context.Ansible()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Ansible.Extensions;

using ModularPipelines.Ansible.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Ansible().Execute(

            new AnsibleExecuteOptions("value"),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command | Options record          |
| ----------- | ----------------------- |
| `ansible`   | `AnsibleExecuteOptions` |

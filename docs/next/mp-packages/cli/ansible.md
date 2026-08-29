# ansible CLI reference

`ModularPipelines.Ansible` provides strongly typed access to the `ansible` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `ansible` executable. Install it separately and ensure `ansible` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Ansible
```

Resolve the service with `context.Tools.Ansible`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Ansible.Services.IAnsible>()` instead.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Ansible.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Ansible.ExecuteAsync(

            new AnsibleExecuteOptions("localhost")

            {

                ListHosts = true,

            },

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command | Options record          |
| ----------- | ----------------------- |
| `ansible`   | `AnsibleExecuteOptions` |

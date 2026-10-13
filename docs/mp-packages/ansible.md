# Ansible Package

`ModularPipelines.Ansible` provides strongly typed access to Ansible's ad-hoc command CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Ansible
```

The `ansible` executable must be installed and available on `PATH` when the pipeline runs.

## Run an ad-hoc command[​](#run-an-ad-hoc-command "Direct link to Run an ad-hoc command")

```
using ModularPipelines.Ansible.Options;



var result = await context.Tools.Ansible.ExecuteAsync(

    new AnsibleExecuteOptions("webservers")

    {

        Inventory = ["hosts.ini"],

        ModuleName = "ping",

        Forks = 10,

    },

    cancellationToken: cancellationToken);
```

This renders the equivalent of:

```
ansible webservers --forks 10 --inventory hosts.ini --module-name ping
```

Options that accept repeated values, including `Inventory`, `ExtraVars`, `ModulePath`, and `VaultId`, accept collections. Password-file and private-key properties are marked as secrets, so Modular Pipelines masks their values in command logs.

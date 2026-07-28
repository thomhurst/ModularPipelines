---
title: Ansible Package
---

# Ansible Package

`ModularPipelines.Ansible` provides strongly typed access to Ansible's ad-hoc command CLI.

## Installation

```shell
dotnet add package ModularPipelines.Ansible
```

The `ansible` executable must be installed and available on `PATH` when the pipeline runs.

## Run an ad-hoc command

```csharp
using ModularPipelines.Ansible.Extensions;
using ModularPipelines.Ansible.Options;

var result = await context.Ansible().Execute(
    new AnsibleExecuteOptions("webservers")
    {
        Inventory = ["hosts.ini"],
        ModuleName = "ping",
        Forks = 10,
    },
    cancellationToken: cancellationToken);
```

This renders the equivalent of:

```shell
ansible webservers --forks 10 --inventory hosts.ini --module-name ping
```

Options that accept repeated values, including `Inventory`, `ExtraVars`, `ModulePath`, and
`VaultId`, accept collections. Password-file and private-key properties are marked as secrets,
so Modular Pipelines masks their values in command logs.

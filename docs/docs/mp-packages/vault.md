---
title: Vault Package
---

# Vault Package

Strongly typed HashiCorp Vault commands.

## Installation

```shell
dotnet add package ModularPipelines.Vault
```

Required command-line tool: `vault`. It must be installed and available on `PATH` when the pipeline runs.

## Context entry points

Use the discoverable `context.Tools` surface from a module:

- `context.Tools.Vault`

## Module example

```csharp

public class UseVaultModule : SyncModule<None>
{
    protected override None Execute(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        var vault = context.Tools.Vault;

        // Call the integration's strongly typed operations here.
        context.Logger.LogInformation("Vault integration is ready");
        return None.Value;
    }
}
```

The package exposes generated options records for its supported CLI commands.


## Connection options and command scope

Vault 2.1.1 advertises HTTP settings on individual commands. `VaultOptions` does
not promote them universally: for example, `vault print token` accepts no such
flags, while output settings vary between commands. Environment variables such
as `VAULT_TOKEN` are not generated as nonexistent CLI options.

```csharp
var options = new VaultReadOptions("secret/example")
{
    Address = "https://vault.example",
    Namespace = "engineering",
    Format = "json",
};
// vault read -address=https://vault.example -namespace=engineering -format=json secret/example
```

Flags follow the command path and precede operands. `Header` and `Mfa` accept
multiple values and repeat their switches. `Namespace` also retains the CLI's
`-ns` alias. Single-hyphen names and equals-separated values follow Vault's parser.

The regenerated API restores options previously missed by multiline help parsing.
Each command retains only its advertised options; server configuration does not
become a setting on unrelated command records. Existing command and operand
names remain available.

Authentication operands, MFA, headers, namespace unlock keys, root-token decoding
values, and OTPs are marked secret for command logging. Certificate and client-key
**file paths** remain visible; they do not contain the file contents. Supply token
credentials through the supported authentication command or environment settings.

See the [Vault CLI documentation](https://developer.hashicorp.com/vault/docs/commands)
and [generated reference](cli/vault.md).

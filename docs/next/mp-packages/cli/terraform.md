# terraform CLI reference

`ModularPipelines.Terraform` provides strongly typed access to the `terraform` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `terraform` executable. Install it separately and ensure `terraform` is available on `PATH`.

See the [terraform installation guide](https://developer.hashicorp.com/terraform/install).

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Terraform
```

Resolve the service with `context.Tools.Terraform`. For projects older than C# 14, import `ModularPipelines.Terraform.Extensions` and use the `context.Terraform()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Terraform.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Terraform.ValidateAsync(

            new TerraformValidateOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                                           | Options record                                         |
| ----------------------------------------------------- | ------------------------------------------------------ |
| `terraform apply`                                     | `TerraformApplyOptions`                                |
| `terraform console`                                   | `TerraformConsoleOptions`                              |
| `terraform destroy`                                   | `TerraformDestroyOptions`                              |
| `terraform fmt`                                       | `TerraformFmtOptions`                                  |
| `terraform force-unlock`                              | `TerraformForceUnlockOptions`                          |
| `terraform get`                                       | `TerraformGetOptions`                                  |
| `terraform graph`                                     | `TerraformGraphOptions`                                |
| `terraform import`                                    | `TerraformImportOptions`                               |
| `terraform init`                                      | `TerraformInitOptions`                                 |
| `terraform login`                                     | `TerraformLoginOptions`                                |
| `terraform logout`                                    | `TerraformLogoutOptions`                               |
| `terraform metadata`                                  | `TerraformMetadataOptions`                             |
| `terraform modules`                                   | `TerraformModulesOptions`                              |
| `terraform output`                                    | `TerraformOutputOptions`                               |
| `terraform plan`                                      | `TerraformPlanOptions`                                 |
| `terraform providers`                                 | `TerraformProvidersOptions`                            |
| `terraform providers lock`                            | `TerraformProvidersLockOptions`                        |
| `terraform providers mirror`                          | `TerraformProvidersMirrorOptions`                      |
| `terraform providers schema`                          | `TerraformProvidersSchemaOptions`                      |
| `terraform query`                                     | `TerraformQueryOptions`                                |
| `terraform refresh`                                   | `TerraformRefreshOptions`                              |
| `terraform show`                                      | `TerraformShowOptions`                                 |
| `terraform stacks`                                    | `TerraformStacksOptions`                               |
| `terraform stacks configuration`                      | `TerraformStacksConfigurationOptions`                  |
| `terraform stacks configuration fetch`                | `TerraformStacksConfigurationFetchOptions`             |
| `terraform stacks configuration list`                 | `TerraformStacksConfigurationListOptions`              |
| `terraform stacks configuration show`                 | `TerraformStacksConfigurationShowOptions`              |
| `terraform stacks configuration upload`               | `TerraformStacksConfigurationUploadOptions`            |
| `terraform stacks configuration watch`                | `TerraformStacksConfigurationWatchOptions`             |
| `terraform stacks create`                             | `TerraformStacksCreateOptions`                         |
| `terraform stacks deployment-group`                   | `TerraformStacksDeploymentGroupOptions`                |
| `terraform stacks deployment-group approve-all-plans` | `TerraformStacksDeploymentGroupApproveAllPlansOptions` |
| `terraform stacks deployment-group list`              | `TerraformStacksDeploymentGroupListOptions`            |
| `terraform stacks deployment-group rerun`             | `TerraformStacksDeploymentGroupRerunOptions`           |
| `terraform stacks deployment-group show`              | `TerraformStacksDeploymentGroupShowOptions`            |
| `terraform stacks deployment-group watch`             | `TerraformStacksDeploymentGroupWatchOptions`           |
| `terraform stacks deployment-run`                     | `TerraformStacksDeploymentRunOptions`                  |
| `terraform stacks deployment-run approve-all-plans`   | `TerraformStacksDeploymentRunApproveAllPlansOptions`   |
| `terraform stacks deployment-run cancel`              | `TerraformStacksDeploymentRunCancelOptions`            |
| `terraform stacks deployment-run list`                | `TerraformStacksDeploymentRunListOptions`              |
| `terraform stacks deployment-run show`                | `TerraformStacksDeploymentRunShowOptions`              |
| `terraform stacks deployment-run watch`               | `TerraformStacksDeploymentRunWatchOptions`             |
| `terraform stacks fmt`                                | `TerraformStacksFmtOptions`                            |
| `terraform stacks init`                               | `TerraformStacksInitOptions`                           |
| `terraform stacks list`                               | `TerraformStacksListOptions`                           |
| `terraform stacks providers-lock`                     | `TerraformStacksProvidersLockOptions`                  |
| `terraform stacks validate`                           | `TerraformStacksValidateOptions`                       |
| `terraform state`                                     | `TerraformStateOptions`                                |
| `terraform state identities`                          | `TerraformStateIdentitiesOptions`                      |
| `terraform state list`                                | `TerraformStateListOptions`                            |
| `terraform state mv`                                  | `TerraformStateMvOptions`                              |
| `terraform state pull`                                | `TerraformStatePullOptions`                            |
| `terraform state push`                                | `TerraformStatePushOptions`                            |
| `terraform state replace-provider`                    | `TerraformStateReplaceProviderOptions`                 |
| `terraform state rm`                                  | `TerraformStateRmOptions`                              |
| `terraform state show`                                | `TerraformStateShowOptions`                            |
| `terraform taint`                                     | `TerraformTaintOptions`                                |
| `terraform test`                                      | `TerraformTestOptions`                                 |
| `terraform untaint`                                   | `TerraformUntaintOptions`                              |
| `terraform validate`                                  | `TerraformValidateOptions`                             |
| `terraform workspace delete`                          | `TerraformWorkspaceDeleteOptions`                      |
| `terraform workspace new`                             | `TerraformWorkspaceNewOptions`                         |
| `terraform workspace select`                          | `TerraformWorkspaceSelectOptions`                      |

# Generate build server files

The `ModularPipelines` package can create the minimal YAML needed to run a pipeline project in GitHub Actions, GitLab CI, or Azure Pipelines.

Add the build system to the pipeline project's `.csproj`:

```
<PropertyGroup>

  <ModularPipelinesBuildSystem>GitHubActions</ModularPipelinesBuildSystem>

</PropertyGroup>
```

The next build creates the provider's conventional file:

| Value            | Generated file                            |
| ---------------- | ----------------------------------------- |
| `GitHubActions`  | `.github/workflows/modular-pipelines.yml` |
| `GitLab`         | `.gitlab-ci.yml`                          |
| `AzurePipelines` | `azure-pipelines.yml`                     |

The generated job installs .NET and runs the pipeline project in Release configuration. Commit the generated YAML to the repository.

## Azure Repos pull request validation[​](#azure-repos-pull-request-validation "Direct link to Azure Repos pull request validation")

Azure Repos Git does not support YAML `pr` triggers. After creating the Azure Pipeline, configure it as an automatic **Build validation** branch policy for the target branch. The generated `pr` block applies only when Azure Pipelines builds a GitHub or Bitbucket Cloud repository. See [Set build validation](https://learn.microsoft.com/azure/devops/repos/git/branch-policies?view=azure-devops#set-build-validation).

## Pipeline projects below the repository root[​](#pipeline-projects-below-the-repository-root "Direct link to Pipeline projects below the repository root")

Generation defaults to the directory containing the pipeline project. If that project is in a subdirectory, set the repository root:

```
<PropertyGroup>

  <ModularPipelinesBuildSystem>GitHubActions</ModularPipelinesBuildSystem>

  <ModularPipelinesRepositoryRoot>$(MSBuildProjectDirectory)/../..</ModularPipelinesRepositoryRoot>

</PropertyGroup>
```

The project path in the generated command is calculated relative to this root. Override it explicitly when needed:

```
<ModularPipelinesPipelineProject>build/MyPipeline/MyPipeline.csproj</ModularPipelinesPipelineProject>
```

## Customization and overwrite safety[​](#customization-and-overwrite-safety "Direct link to Customization and overwrite safety")

The following optional properties customize the generated file:

| Property                          | Default                                                  |
| --------------------------------- | -------------------------------------------------------- |
| `ModularPipelinesBuildBranch`     | `main`                                                   |
| `ModularPipelinesDotNetVersion`   | `10.0.x`                                                 |
| `ModularPipelinesDotNetSdkImage`  | `mcr.microsoft.com/dotnet/sdk:10.0`                      |
| `ModularPipelinesTargetFramework` | `TargetFramework`, or the first `TargetFrameworks` entry |

For a multi-target pipeline project, set `ModularPipelinesTargetFramework` to choose which target framework the generated job runs.

Existing YAML is preserved. To intentionally regenerate and replace it, build once with:

```
dotnet build -p:ModularPipelinesOverwriteBuildServerFiles=true
```

Remove `ModularPipelinesBuildSystem` after committing the generated file if no further regeneration is wanted.

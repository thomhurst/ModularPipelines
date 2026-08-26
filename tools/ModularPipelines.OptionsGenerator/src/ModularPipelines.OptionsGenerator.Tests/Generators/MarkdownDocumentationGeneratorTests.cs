using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class MarkdownDocumentationGeneratorTests
{
    [Test]
    public async Task GenerateAsync_EmitsInstallEntryPointCommandsAndWorkedExample()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake-cli",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands =
            [
                new CliCommandDefinition
                {
                    FullCommand = "fake-cli run",
                    CommandParts = ["run"],
                    ClassName = "FakeRunOptions",
                    ParentClassName = "FakeOptions",
                    ToolNamespacePrefix = "Fake",
                    Options =
                    [
                        new CliOptionDefinition
                        {
                            SwitchName = "--project",
                            PropertyName = "Project",
                            CSharpType = "string",
                            IsRequired = true,
                        },
                    ],
                    IsSafeForDocumentation = true,
                    DocumentationExampleValues = new Dictionary<string, string>
                    {
                        ["Project"] = "\"sample.csproj\"",
                    },
                },
            ],
            PreferredDocumentationExampleCommand = "fake-cli run",
        };

        var files = await new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(files).Count().IsEqualTo(1);
        await Assert.That(files[0].RelativePath.Replace('\\', '/'))
            .IsEqualTo("docs/docs/mp-packages/cli/fake-cli.md");
        await Assert.That(files[0].Content).Contains("dotnet add package ModularPipelines.Fake");
        await Assert.That(files[0].Content)
            .Contains("This package does not install the `fake-cli` executable");
        await Assert.That(files[0].Content).Contains("`fake-cli` is available on `PATH`");
        await Assert.That(files[0].Content).Contains("context.Tools.Fake");
        await Assert.That(files[0].Content).Contains("compatibility fallback");
        await Assert.That(files[0].Content)
            .Contains("import `ModularPipelines.Fake.Extensions`");
        await Assert.That(files[0].Content)
            .DoesNotContain("using ModularPipelines.Fake.Extensions;");
        await Assert.That(files[0].Content).Contains("using ModularPipelines.Context;");
        await Assert.That(files[0].Content).Contains("using ModularPipelines.Models;");
        await Assert.That(files[0].Content).Contains("using ModularPipelines.Modules;");
        await Assert.That(files[0].Content).Contains("Module<CommandResult>");
        await Assert.That(files[0].Content).Contains("Task<CommandResult> ExecuteAsync");
        await Assert.That(files[0].Content).Contains("return await context.Tools.Fake");
        await Assert.That(files[0].Content).Contains("| `fake-cli run` | `FakeRunOptions` |");
        await Assert.That(files[0].Content).Contains("new FakeRunOptions(\"sample.csproj\")");
    }

    [Test]
    [Arguments(
        "syft",
        "https://oss.anchore.com/docs/installation/syft/")]
    [Arguments(
        "sonar-scanner",
        "https://docs.sonarsource.com/sonarqube-server/analyzing-source-code/scanners/sonarscanner")]
    [Arguments(
        "terraform",
        "https://developer.hashicorp.com/terraform/install")]
    public async Task GenerateAsync_EmitsToolSpecificExecutablePrerequisites(
        string toolName,
        string installationUrl)
    {
        var documentation = await GenerateDocumentation(ToolDefinition(toolName));

        await Assert.That(documentation).Contains($"does not install the `{toolName}` executable");
        await Assert.That(documentation).Contains($"[{toolName} installation guide]({installationUrl})");
    }

    [Test]
    public async Task GenerateAsync_EmitsPinnedGeneratorVersion()
    {
        var documentation = await GenerateDocumentation(ToolDefinition("sonar-scanner"));

        await Assert.That(documentation)
            .Contains("generation workflow is pinned to `sonar-scanner` version `8.0.1.6346`");
    }

    [Test]
    public async Task GenerateAsync_UsesSafeGenericExecutableFallback()
    {
        var documentation = await GenerateDocumentation(ToolDefinition("future-cli"));

        await Assert.That(documentation).Contains("does not install the `future-cli` executable");
        await Assert.That(documentation)
            .Contains("Follow the executable's official documentation for installation instructions.");
    }

    [Test]
    public async Task GenerateAsync_UsesExplicitPrerequisiteMetadata()
    {
        var tool = ToolDefinition("custom-cli") with
        {
            ExecutablePrerequisite = new CliExecutablePrerequisite
            {
                CommandName = "custom",
                SupportedVersion = "2.4.1",
                InstallationUrl = "https://example.test/custom/install",
                InstallationNotes = "Install the platform-specific archive.",
            },
        };

        var documentation = await GenerateDocumentation(tool);

        await Assert.That(documentation).Contains("does not install the `custom` executable");
        await Assert.That(documentation).Contains("version `2.4.1`");
        await Assert.That(documentation).Contains("https://example.test/custom/install");
        await Assert.That(documentation).Contains("Install the platform-specific archive.");
    }

    [Test]
    public async Task GenerateAsync_AcceptsExplicitMetadataExemption()
    {
        var tool = ToolDefinition("embedded") with
        {
            ExecutablePrerequisiteMetadataExemption =
                "This integration is supplied by the host environment.",
        };

        var documentation = await GenerateDocumentation(tool);

        await Assert.That(documentation).Contains("does not install the `embedded` executable");
        await Assert.That(documentation)
            .Contains("This integration is supplied by the host environment.");
    }

    [Test]
    public async Task GenerateAsync_IncludesRootCommandsExposedBySubDomainExecute()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands =
            [
                Command("fake app", "FakeAppOptions", ["app"]),
                Command("fake app get", "FakeAppGetOptions", ["app", "get"], "App"),
            ],
        };

        var files = await new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(files[0].Content).Contains("| `fake app` | `FakeAppOptions` |");
        await Assert.That(files[0].Content).Contains("| `fake app get` | `FakeAppGetOptions` |");
    }

    [Test]
    public async Task GenerateAsync_Omits_Compatibility_Only_Commands()
    {
        var tool = Tool(
            "fake",
            Command("fake current", "FakeCurrentOptions", ["current"]),
            Command("fake removed", "FakeRemovedOptions", ["removed"]) with
            {
                IsCompatibilityOnly = true,
            });

        var files = await new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(files[0].Content).Contains("| `fake current` | `FakeCurrentOptions` |");
        await Assert.That(files[0].Content).DoesNotContain("fake removed");
    }

    [Test]
    public async Task GenerateAsync_Rejects_Compatibility_Only_Preferred_Command()
    {
        var removedCommand = Command("fake removed", "FakeRemovedOptions", ["removed"]) with
        {
            IsCompatibilityOnly = true,
            IsSafeForDocumentation = true,
        };
        var tool = Tool(
            "fake",
            Command("fake current", "FakeCurrentOptions", ["current"]),
            removedCommand) with
        {
            PreferredDocumentationExampleCommand = removedCommand.FullCommand,
        };

        void GenerateDocumentation() =>
            _ = new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(GenerateDocumentation)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("does not match an emitted command");
    }

    [Test]
    public async Task GenerateAsync_UsesExecuteForCollidingPreferredCommands()
    {
        var rootCommand = Command("fake app", "FakeAppOptions", ["app"]) with
        {
            IsSafeForDocumentation = true,
        };
        var rootCollisionTool = Tool(
            "fake",
            rootCommand,
            Command("fake app get", "FakeAppGetOptions", ["app", "get"], "app")) with
        {
            PreferredDocumentationExampleCommand = rootCommand.FullCommand,
        };

        var nestedCommand = Command(
            "fake app get",
            "FakeAppGetOptions",
            ["app", "get"],
            "app") with
        {
            IsSafeForDocumentation = true,
        };
        var nestedCollisionTool = Tool(
            "fake",
            nestedCommand,
            Command(
                "fake app get value",
                "FakeAppGetValueOptions",
                ["app", "get", "value"],
                "app")) with
        {
            PreferredDocumentationExampleCommand = nestedCommand.FullCommand,
        };

        var executeCommand = Command(
            "fake app execute",
            "FakeAppExecuteOptions",
            ["app", "execute"],
            "app") with
        {
            IsSafeForDocumentation = true,
        };
        var executeCollisionTool = Tool(
            "fake",
            Command("fake app", "FakeAppOptions", ["app"]),
            executeCommand) with
        {
            PreferredDocumentationExampleCommand = executeCommand.FullCommand,
        };

        var testCases = new[]
        {
            (Tool: rootCollisionTool,
                Invocation: "context.Tools.Fake.App.ExecuteAsync(",
                Method: "ExecuteAsync(",
                ServiceFile: "FakeApp.Generated.cs",
                OptionsType: "FakeAppOptions"),
            (Tool: nestedCollisionTool,
                Invocation: "context.Tools.Fake.App.Get.ExecuteAsync(",
                Method: "ExecuteAsync(",
                ServiceFile: "FakeAppGet.Generated.cs",
                OptionsType: "FakeAppGetOptions"),
            (Tool: executeCollisionTool,
                Invocation: "context.Tools.Fake.App.ExecuteCommandAsync(",
                Method: "ExecuteCommandAsync(",
                ServiceFile: "FakeApp.Generated.cs",
                OptionsType: "FakeAppExecuteOptions"),
        };

        foreach (var testCase in testCases)
        {
            var documentation = await GenerateDocumentation(testCase.Tool);
            var serviceFiles = await new SubDomainClassGenerator().GenerateAsync(testCase.Tool);
            var collisionService = serviceFiles.Single(file =>
                Path.GetFileName(file.RelativePath) == testCase.ServiceFile);

            await Assert.That(documentation).Contains(testCase.Invocation);
            await Assert.That(collisionService.Content)
                .Contains($"Task<CommandResult> {testCase.Method}");
            await Assert.That(collisionService.Content).Contains(testCase.OptionsType);
            await AssertDocumentationExampleCompiles(testCase.Tool);
        }
    }

    [Test]
    public async Task GenerateAsync_MatchesPascalizedParentCollisions()
    {
        var executeCommand = Command(
            "fake app service_accounts execute",
            "FakeAppServiceAccountsExecuteOptions",
            ["app", "service_accounts", "execute"],
            "app") with
        {
            IsSafeForDocumentation = true,
        };
        var tool = Tool(
            "fake",
            Command(
                "fake app service-accounts",
                "FakeAppServiceAccountsOptions",
                ["app", "service-accounts"],
                "app"),
            executeCommand) with
        {
            PreferredDocumentationExampleCommand = executeCommand.FullCommand,
        };

        var documentation = await GenerateDocumentation(tool);
        var serviceFiles = await new SubDomainClassGenerator().GenerateAsync(tool);
        var service = serviceFiles.Single(file =>
            Path.GetFileName(file.RelativePath) == "FakeAppServiceAccounts.Generated.cs");

        await Assert.That(documentation)
            .Contains("context.Tools.Fake.App.ServiceAccounts.ExecuteCommandAsync(");
        await Assert.That(service.Content)
            .Contains("Task<CommandResult> ExecuteCommandAsync(");
    }

    [Test]
    public async Task GenerateAsync_Ignores_Siblings_Moved_To_Execute()
    {
        var asyncCommand = Command(
            "fake app foo-bar-async",
            "FakeAppFooBarAsyncOptions",
            ["app", "foo-bar-async"],
            "app") with
        {
            IsSafeForDocumentation = true,
        };
        var tool = Tool(
            "fake",
            Command("fake app foo-bar", "FakeAppFooBarOptions", ["app", "foo-bar"], "app"),
            Command(
                "fake app foo_bar child",
                "FakeAppFooChildOptions",
                ["app", "foo_bar", "child"],
                "app"),
            asyncCommand) with
        {
            PreferredDocumentationExampleCommand = asyncCommand.FullCommand,
        };

        var documentation = await GenerateDocumentation(tool);
        var service = (await new SubDomainClassGenerator().GenerateAsync(tool))
            .Single(file => Path.GetFileName(file.RelativePath) == "FakeApp.Generated.cs")
            .Content;

        await Assert.That(documentation)
            .Contains("context.Tools.Fake.App.FooBarAsync(");
        await Assert.That(documentation)
            .DoesNotContain("FooBarAsyncCommandAsync(");
        await Assert.That(service)
            .Contains("Task<CommandResult> FooBarAsync(");
    }

    [Test]
    public async Task GenerateAsync_UsesTheGeneratedConstructorParameterList()
    {
        var command = Command("fake run", "FakeRunOptions", ["run"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--target",
                    PropertyName = "Target",
                    CSharpType = "int",
                    IsRequired = true,
                },
            ],
            PositionalArguments =
            [
                new CliPositionalArgument
                {
                    PropertyName = "target",
                    CSharpType = "string",
                    IsRequired = true,
                },
                new CliPositionalArgument
                {
                    PropertyName = "Files",
                    CSharpType = "string",
                    IsRequired = true,
                },
                new CliPositionalArgument
                {
                    PropertyName = "files",
                    CSharpType = "IEnumerable<string>",
                    IsRequired = true,
                },
            ],
            IsSafeForDocumentation = true,
            DocumentationExampleValues = new Dictionary<string, string>
            {
                ["Target"] = "42",
                ["Files"] = "[\"input.txt\"]",
            },
        };
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands = [command],
            PreferredDocumentationExampleCommand = "fake run",
        };

        var documentation = await new MarkdownDocumentationGenerator().GenerateAsync(tool);
        var options = await new OptionsClassGenerator().GenerateAsync(tool);

        await Assert.That(documentation[0].Content).Contains("new FakeRunOptions(42, [\"input.txt\"])");
        await Assert.That(options[0].Content).Contains("int Target");
        await Assert.That(options[0].Content).Contains("IEnumerable<string> Files");
        await Assert.That(options[0].Content).DoesNotContain("string target");
    }

    [Test]
    public async Task GenerateAsync_SelectsExampleCommandDeterministically()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands =
            [
                Command("fake zeta", "FakeZetaOptions", ["zeta"]) with
                {
                    IsSafeForDocumentation = true,
                },
                Command("fake alpha", "FakeAlphaOptions", ["alpha"]) with
                {
                    IsSafeForDocumentation = true,
                },
            ],
            PreferredDocumentationExampleCommand = "fake zeta",
        };

        var documentation = await new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(documentation[0].Content).Contains("context.Tools.Fake.ZetaAsync(");
        await Assert.That(documentation[0].Content).DoesNotContain("context.Tools.Fake.AlphaAsync(");
    }

    [Test]
    public async Task GenerateAsync_Documents_Supplemental_Global_Options()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands = [Command("fake run", "FakeRunOptions", ["run"])],
            SupplementalGlobalOptions =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--license-key",
                    PropertyName = "LicenseKey",
                    CSharpType = "string?",
                    Description = "Enables licensed features.",
                    DocumentationUrl = "https://example.test/license",
                    Availability = "Secure edition",
                    IsSecret = true,
                    ValueSeparator = "=",
                },
            ],
        };

        var documentation = await new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(documentation[0].Content).Contains("## Global options");
        await Assert.That(documentation[0].Content)
            .Contains("[`--license-key`](https://example.test/license)");
        await Assert.That(documentation[0].Content).Contains("`LicenseKey`");
        await Assert.That(documentation[0].Content).Contains("Secure edition");
        await Assert.That(documentation[0].Content).Contains("rendered before the selected subcommand");
    }

    [Test]
    public async Task GenerateAsync_DocumentsMachineReadableCoverageExclusions()
    {
        var tool = new CliToolDefinition
        {
            ToolName = "fake",
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands = [Command("fake run", "FakeRunOptions", ["run"])],
            CommandCoverage = new CliCommandCoveragePolicy
            {
                Exclusions =
                [
                    new CliCommandCoverageExclusion
                    {
                        Command = "fake enterprise",
                        Reason = "Requires an enterprise license.",
                    },
                ],
            },
        };

        var documentation = await new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(documentation[0].Content).Contains("## Intentionally excluded commands");
        await Assert.That(documentation[0].Content)
            .Contains("| `fake enterprise` | Requires an enterprise license. |");
    }

    [Test]
    public async Task GenerateAsync_OmitsRunnableExampleWithoutQualifiedMetadata()
    {
        var tool = Tool("fake", Command("fake delete", "FakeDeleteOptions", ["delete"]));

        var documentation = await new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(documentation[0].Content)
            .Contains("A runnable example is omitted when no command has complete safety metadata");
        await Assert.That(documentation[0].Content).DoesNotContain("context.Tools.Fake.DeleteAsync(");
    }

    [Test]
    public async Task GenerateAsync_RejectsMissingPreferredCommand()
    {
        var tool = Tool("fake", Command("fake run", "FakeRunOptions", ["run"])) with
        {
            PreferredDocumentationExampleCommand = "fake status",
        };

        void GenerateDocumentation() =>
            _ = new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(GenerateDocumentation)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("'fake status' for 'fake' does not match an emitted command");
    }

    [Test]
    public async Task Apply_RejectsCatalogCommandsMissingFromScraperOutput()
    {
        var tool = Tool(
            "vault",
            Command("vault state", "VaultStateOptions", ["state"]));

        void ApplyCatalog() => DocumentationExampleCatalog.Apply(tool);

        await Assert.That(ApplyCatalog)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining(
                "catalog for 'vault' references missing command(s): vault delete, vault status");
    }

    [Test]
    public async Task ValidateRegisteredTools_CoversEveryRegisteredCli()
    {
        var registeredTools = new[]
        {
            "ansible", "argocd", "aws", "az", "brew", "buildah", "cargo", "choco",
            "cosign", "docker", "dotnet", "eksctl", "flux", "flyway", "gcloud", "gh",
            "git", "go", "gradle", "grype", "hadolint", "helm", "jq", "kind", "kubectl",
            "kustomize", "liquibase", "minikube", "mvn", "newman", "packer", "pip", "pnpm",
            "podman", "pulumi", "shellcheck", "skopeo", "snyk", "sonar-scanner", "syft",
            "terraform", "trivy", "vault", "winget", "yarn", "yq",
        };

        void ValidateCatalog() =>
            DocumentationExampleCatalog.ValidateRegisteredTools(registeredTools);

        await Assert.That(ValidateCatalog).ThrowsNothing();
    }

    [Test]
    public async Task ValidateRegisteredTools_RejectsUnclassifiedCli()
    {
        void ValidateCatalog() =>
            DocumentationExampleCatalog.ValidateRegisteredTools(["unclassified"]);

        await Assert.That(ValidateCatalog)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("unclassified");
    }

    [Test]
    public async Task GenerateAsync_EnforcesIntentionalCatalogOmission()
    {
        var tool = Tool(
            "aws",
            Command("aws version", "AwsVersionOptions", ["version"]) with
            {
                IsSafeForDocumentation = true,
            }) with
        {
            PreferredDocumentationExampleCommand = "aws version",
        };

        var documentation = await new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(documentation[0].Content)
            .Contains("A runnable example is omitted when no command has complete safety metadata");
        await Assert.That(documentation[0].Content)
            .DoesNotContain("context.Tools.Fake.VersionAsync(");
    }

    [Test]
    public async Task GenerateAsync_RejectsIncompleteExampleValues()
    {
        var command = Command("fake run", "FakeRunOptions", ["run"]) with
        {
            Options =
            [
                new CliOptionDefinition
                {
                    SwitchName = "--project",
                    PropertyName = "Project",
                    CSharpType = "string",
                    IsRequired = true,
                },
            ],
            IsSafeForDocumentation = true,
        };
        var tool = Tool("fake", command) with
        {
            PreferredDocumentationExampleCommand = "fake run",
        };

        void GenerateDocumentation() =>
            _ = new MarkdownDocumentationGenerator().GenerateAsync(tool);

        await Assert.That(GenerateDocumentation)
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("required property 'Project' has no sample value");
    }

    [Test]
    public async Task GenerateAsync_UsesCuratedSafeRegressionExamples()
    {
        var ansible = Tool(
            "ansible",
            Command("ansible", "AnsibleExecuteOptions", []) with
            {
                Options =
                [
                    Option("--list-hosts", "ListHosts", "bool?"),
                ],
                PositionalArguments =
                [
                    Positional("Pattern", "string", isRequired: true),
                ],
            });
        var buildah = Tool(
            "buildah",
            Command("buildah add", "BuildahAddOptions", ["add"]),
            Command("buildah containers", "BuildahContainersOptions", ["containers"]));
        var jq = Tool(
            "jq",
            Command("jq", "JqExecuteOptions", []) with
            {
                PositionalArguments =
                [
                    Positional("Filter", "string?"),
                    Positional("InputFiles", "IEnumerable<string>?"),
                ],
            });
        var newman = Tool(
            "newman",
            Command("newman run", "NewmanRunOptions", ["run"]) with
            {
                PositionalArguments =
                [
                    Positional("Collection", "string", isRequired: true),
                ],
            });
        var packer = Tool(
            "packer",
            Command("packer console", "PackerConsoleOptions", ["console"]));
        var vault = Tool(
            "vault",
            Command("vault delete", "VaultDeleteOptions", ["delete"]),
            Command("vault status", "VaultStatusOptions", ["status"]));

        var generator = new MarkdownDocumentationGenerator();
        var ansibleDocumentation = (await generator.GenerateAsync(ansible))[0].Content;
        var buildahDocumentation = (await generator.GenerateAsync(buildah))[0].Content;
        var jqDocumentation = (await generator.GenerateAsync(jq))[0].Content;
        var newmanDocumentation = (await generator.GenerateAsync(newman))[0].Content;
        var packerDocumentation = (await generator.GenerateAsync(packer))[0].Content;
        var vaultDocumentation = (await generator.GenerateAsync(vault))[0].Content;

        await Assert.That(ansibleDocumentation).Contains("new AnsibleExecuteOptions(\"localhost\")");
        await Assert.That(ansibleDocumentation).Contains("ListHosts = true");
        await Assert.That(buildahDocumentation).Contains("context.Tools.Fake.ContainersAsync(");
        await Assert.That(buildahDocumentation).DoesNotContain("context.Tools.Fake.AddAsync(");
        await Assert.That(jqDocumentation).Contains("Filter = \".\"");
        await Assert.That(jqDocumentation).Contains("InputFiles = [\"input.json\"]");
        await Assert.That(newmanDocumentation)
            .Contains("Unsafe or destructive commands do not receive runnable examples");
        await Assert.That(newmanDocumentation).DoesNotContain("context.Tools.Fake.RunAsync(");
        await Assert.That(packerDocumentation).DoesNotContain("context.Tools.Fake.ConsoleAsync(");
        await Assert.That(vaultDocumentation).Contains("context.Tools.Fake.StatusAsync(");
        await Assert.That(vaultDocumentation).DoesNotContain("context.Tools.Fake.DeleteAsync(");

        foreach (var tool in new[] { ansible, buildah, jq, vault })
        {
            await AssertDocumentationExampleCompiles(tool);
        }
    }

    [Test]
    public async Task GenerateAsync_UsesCuratedExamplesForPopularTools()
    {
        var testCases = new[]
        {
            (Tool("az", Command("az account list", "AzAccountListOptions", ["account", "list"], "account")),
                "context.Tools.Fake.Account.ListAsync("),
            (Tool("cargo", Command("cargo check", "CargoCheckOptions", ["check"]) with
                {
                    Options = [Option("--quiet", "Quiet", "bool?")],
                }),
                "context.Tools.Fake.CheckAsync("),
            (Tool("docker", Command("docker info", "DockerInfoOptions", ["info"])),
                "context.Tools.Fake.InfoAsync("),
            (Tool("dotnet", Command("dotnet workload list", "DotNetWorkloadListOptions", ["workload", "list"], "workload")),
                "context.Tools.Fake.Workload.ListAsync("),
            (Tool("gcloud", Command("gcloud info", "GcloudInfoOptions", ["info"]) with
                {
                    Options = [Option("--anonymize", "Anonymize", "bool?")],
                }),
                "context.Tools.Fake.InfoAsync("),
            (Tool("gh", Command("gh config list", "GhConfigListOptions", ["config", "list"], "config")),
                "context.Tools.Fake.Config.ListAsync("),
            (Tool("git", Command("git status", "GitStatusOptions", ["status"]) with
                {
                    Options = [Option("--short", "Short", "bool?")],
                }),
                "context.Tools.Fake.StatusAsync("),
            (Tool("go", Command("go vet", "GoVetOptions", ["vet"])),
                "context.Tools.Fake.VetAsync("),
            (Tool("helm", Command("helm env", "HelmEnvOptions", ["env"])),
                "context.Tools.Fake.EnvAsync("),
            (Tool("kubectl", Command("kubectl config view", "KubernetesConfigViewOptions", ["config", "view"], "config")),
                "context.Tools.Fake.Config.ViewAsync("),
            (Tool("pip", Command("pip freeze", "PipFreezeOptions", ["freeze"])),
                "context.Tools.Fake.FreezeAsync("),
            (Tool("pnpm", Command("pnpm audit", "PnpmAuditOptions", ["audit"]) with
                {
                    Options = [Option("--audit-level", "AuditLevel", "string?")],
                }),
                "context.Tools.Fake.AuditAsync("),
            (Tool("terraform", Command("terraform validate", "TerraformValidateOptions", ["validate"])),
                "context.Tools.Fake.ValidateAsync("),
        };

        foreach (var (tool, expectedInvocation) in testCases)
        {
            var documentation = (await new MarkdownDocumentationGenerator()
                .GenerateAsync(tool))[0].Content;

            await Assert.That(documentation).Contains(expectedInvocation);
            await AssertDocumentationExampleCompiles(tool);
        }
    }

    private static async Task AssertDocumentationExampleCompiles(CliToolDefinition tool)
    {
        var preparedTool = DocumentationExampleCatalog.Apply(tool);
        var command = preparedTool.Commands.Single(candidate => string.Equals(
            candidate.FullCommand,
            preparedTool.PreferredDocumentationExampleCommand,
            StringComparison.OrdinalIgnoreCase));
        var documentation = (await new MarkdownDocumentationGenerator().GenerateAsync(tool))[0].Content;
        var example = ExtractCSharpExample(documentation);
        var stubs = GenerateCompilationStubs(preparedTool, command);
        var references = ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!)
            .Split(Path.PathSeparator)
            .Select(path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create(
            $"{tool.ToolName}-documentation-example",
            [
                CSharpSyntaxTree.ParseText(
                    "global using System;\n"
                    + "global using System.Collections.Generic;\n"
                    + "global using System.Threading;\n"
                    + "global using System.Threading.Tasks;"),
                CSharpSyntaxTree.ParseText(stubs),
                CSharpSyntaxTree.ParseText(example),
            ],
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        var errors = compilation.GetDiagnostics()
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .Select(diagnostic => diagnostic.ToString())
            .ToArray();

        await Assert.That(errors).IsEmpty();
    }

    private static string ExtractCSharpExample(string documentation)
    {
        const string openingFence = "```csharp\n";
        const string closingFence = "```";
        var normalizedDocumentation = documentation.ReplaceLineEndings("\n");
        var start = normalizedDocumentation.IndexOf(openingFence, StringComparison.Ordinal);
        if (start < 0)
        {
            throw new InvalidOperationException("Documentation has no C# example.");
        }

        start += openingFence.Length;
        var end = normalizedDocumentation.IndexOf(closingFence, start, StringComparison.Ordinal);
        if (end < 0)
        {
            throw new InvalidOperationException("Documentation example fence is not closed.");
        }

        return normalizedDocumentation[start..end];
    }

    private static string GenerateCompilationStubs(
        CliToolDefinition tool,
        CliCommandDefinition command)
    {
        var requiredParameters = GeneratorUtils.GetRequiredConstructorParameters(command);
        var requiredNames = requiredParameters
            .Select(parameter => parameter.PropertyName)
            .ToHashSet(StringComparer.Ordinal);
        var properties = command.Options
            .Select(option => (option.PropertyName, option.CSharpType))
            .Concat(CliPositionalArgument.MergeDuplicates(command.PositionalArguments)
                .Select(argument => (argument.PropertyName, argument.CSharpType)))
            .DistinctBy(property => property.PropertyName, StringComparer.Ordinal)
            .Where(property => !requiredNames.Contains(property.PropertyName))
            .ToArray();
        var constructor = requiredParameters.Count == 0
            ? string.Empty
            : $"({string.Join(", ", requiredParameters.Select(parameter => $"{parameter.CSharpType} {parameter.PropertyName}"))})";
        var propertyDeclarations = string.Join(
            Environment.NewLine,
            properties.Select(property =>
                $"        public {property.CSharpType} {property.PropertyName} {{ get; set; }}"));
        var navigationSegments = GetNavigationSegments(tool, command);
        var methodName = navigationSegments[^1];
        var serviceMembers = GenerateServiceMembers(
            tool,
            command,
            navigationSegments,
            methodName);

        return $$"""
            #nullable enable

            namespace ModularPipelines.Context
            {
                public interface IModuleContext
                {
                    {{tool.TargetNamespace}}.Extensions.I{{tool.NamespacePrefix}}Tools Tools { get; }
                }
            }

            namespace ModularPipelines.Models
            {
                public sealed class CommandResult
                {
                }
            }

            namespace ModularPipelines.Modules
            {
                public abstract class Module<T>
                {
                    protected abstract Task<T> ExecuteAsync(
                        ModularPipelines.Context.IModuleContext context,
                        CancellationToken cancellationToken);
                }
            }

            namespace {{tool.TargetNamespace}}.Options
            {
                public record {{command.ClassName}}{{constructor}}
                {
            {{propertyDeclarations}}
                }
            }

            namespace {{tool.TargetNamespace}}.Extensions
            {
                using ModularPipelines.Context;
                using ModularPipelines.Models;
                using {{tool.TargetNamespace}}.Options;

                public interface I{{tool.NamespacePrefix}}Service
                {
            {{serviceMembers.RootMember}}
                }

                public interface I{{tool.NamespacePrefix}}Tools
                {
                    I{{tool.NamespacePrefix}}Service {{tool.NamespacePrefix}} { get; }
                }

            {{serviceMembers.NavigationTypes}}

                public static class {{tool.NamespacePrefix}}Extensions
                {
                    public static I{{tool.NamespacePrefix}}Service {{tool.NamespacePrefix}}(
                        this IModuleContext context) =>
                        throw new NotImplementedException();
                }
            }
            """;
    }

    private static IReadOnlyList<string> GetNavigationSegments(
        CliToolDefinition tool,
        CliCommandDefinition command)
    {
        var invocation = MarkdownDocumentationGenerator.BuildInvocation(tool, command);
        var prefix = $"context.Tools.{tool.NamespacePrefix}.";
        return invocation[prefix.Length..].Split('.');
    }

    private static (string RootMember, string NavigationTypes) GenerateServiceMembers(
        CliToolDefinition tool,
        CliCommandDefinition command,
        IReadOnlyList<string> navigationSegments,
        string methodName)
    {
        const string methodIndent = "                    ";
        var methodDeclaration =
            $"{methodIndent}Task<CommandResult?> {methodName}(\n"
            + $"{methodIndent}    {command.ClassName} options,\n"
            + $"{methodIndent}    CancellationToken cancellationToken = default);";
        if (navigationSegments.Count == 1)
        {
            return (methodDeclaration, string.Empty);
        }

        methodDeclaration =
            $"{methodIndent}public Task<CommandResult?> {methodName}(\n"
            + $"{methodIndent}    {command.ClassName} options,\n"
            + $"{methodIndent}    CancellationToken cancellationToken = default) =>\n"
            + $"{methodIndent}    throw new NotImplementedException();";
        var propertySegments = navigationSegments.SkipLast(1).ToArray();
        var typeNames = propertySegments
            .Select((_, index) =>
                $"{tool.NamespacePrefix}{string.Concat(propertySegments.Take(index + 1))}Service")
            .ToArray();
        var rootMember = $"{methodIndent}{typeNames[0]} {propertySegments[0]} {{ get; }}";
        var navigationTypes = new StringBuilder();

        for (var index = 0; index < typeNames.Length; index++)
        {
            navigationTypes.AppendLine($"                public sealed class {typeNames[index]}");
            navigationTypes.AppendLine("                {");
            if (index == typeNames.Length - 1)
            {
                navigationTypes.AppendLine(methodDeclaration);
            }
            else
            {
                navigationTypes.AppendLine(
                    $"{methodIndent}public {typeNames[index + 1]} {propertySegments[index + 1]} {{ get; }} = new();");
            }

            navigationTypes.AppendLine("                }");
            navigationTypes.AppendLine();
        }

        return (rootMember, navigationTypes.ToString());
    }

    private static CliToolDefinition Tool(
        string toolName,
        params CliCommandDefinition[] commands) => new()
        {
            ToolName = toolName,
            NamespacePrefix = "Fake",
            TargetNamespace = "ModularPipelines.Fake",
            OutputDirectory = "src/ModularPipelines.Fake",
            Commands = commands,
        };

    private static CliOptionDefinition Option(
        string switchName,
        string propertyName,
        string cSharpType) => new()
        {
            SwitchName = switchName,
            PropertyName = propertyName,
            CSharpType = cSharpType,
        };

    private static CliPositionalArgument Positional(
        string propertyName,
        string cSharpType,
        bool isRequired = false) => new()
        {
            PropertyName = propertyName,
            CSharpType = cSharpType,
            IsRequired = isRequired,
        };

    private static CliCommandDefinition Command(
        string fullCommand,
        string className,
        string[] commandParts,
        string? subDomainGroup = null) => new()
        {
            FullCommand = fullCommand,
            CommandParts = commandParts,
            ClassName = className,
            ParentClassName = "FakeOptions",
            ToolNamespacePrefix = "Fake",
            Options = [],
            SubDomainGroup = subDomainGroup,
        };

    private static CliToolDefinition ToolDefinition(string toolName)
    {
        var command = toolName == "terraform"
            ? Command("terraform validate", "TerraformValidateOptions", ["validate"])
            : Command($"{toolName} status", "FakeStatusOptions", ["status"]);

        return Tool(toolName, command);
    }

    private static async Task<string> GenerateDocumentation(CliToolDefinition tool) =>
        (await new MarkdownDocumentationGenerator().GenerateAsync(tool))[0].Content;
}

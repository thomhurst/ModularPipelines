using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Generators;

/// <summary>
/// Declared executable prerequisites for every registered CLI integration.
/// A missing entry is intentional validation failure, so adding a scraper requires
/// either catalog metadata or an explicit exemption on its tool definition.
/// </summary>
internal static class ExecutablePrerequisiteCatalog
{
    internal const string GenericInstallationNotes =
        "Follow the executable's official documentation for installation instructions.";

    private static readonly IReadOnlyDictionary<string, CliExecutablePrerequisite> Prerequisites =
        new Dictionary<string, CliExecutablePrerequisite>(StringComparer.OrdinalIgnoreCase)
        {
            ["ansible"] = Declared("ansible"),
            ["argocd"] = Declared("argocd"),
            ["aws"] = Declared("aws"),
            ["az"] = Declared("az"),
            ["brew"] = Declared("brew"),
            ["buildah"] = Declared("buildah"),
            ["cargo"] = Declared("cargo"),
            ["choco"] = Declared("choco"),
            ["cosign"] = Declared("cosign"),
            ["docker"] = Declared("docker"),
            ["dotnet"] = Declared("dotnet"),
            ["eksctl"] = Declared("eksctl"),
            ["flyway"] = Declared("flyway"),
            ["flux"] = Declared("flux"),
            ["gcloud"] = Declared("gcloud"),
            ["gh"] = Declared("gh"),
            ["git"] = Declared("git"),
            ["go"] = Declared("go"),
            ["gradle"] = Declared("gradle"),
            ["grype"] = Declared("grype"),
            ["hadolint"] = Declared("hadolint"),
            ["helm"] = Declared("helm"),
            ["jq"] = Declared("jq"),
            ["kind"] = Declared("kind"),
            ["kubectl"] = Declared("kubectl"),
            ["kustomize"] = Declared("kustomize"),
            ["liquibase"] = Declared("liquibase"),
            ["minikube"] = Declared("minikube"),
            ["mvn"] = Declared("mvn"),
            ["nbgv"] = new()
            {
                CommandName = "nbgv",
                SupportedVersion = "3.10.91",
                InstallationUrl =
                    "https://dotnet.github.io/Nerdbank.GitVersioning/docs/nbgv-cli.html",
                InstallationNotes =
                    "Install the nbgv .NET tool globally or in a tool path available on PATH.",
            },
            ["newman"] = Declared("newman"),
            ["npm"] = new()
            {
                CommandName = "npm",
                InstallationUrl = "https://docs.npmjs.com/downloading-and-installing-node-js-and-npm",
                InstallationNotes = "Install npm with a supported Node.js distribution.",
            },
            ["packer"] = Declared("packer"),
            ["pip"] = Declared("pip"),
            ["pnpm"] = Declared("pnpm"),
            ["podman"] = Declared("podman"),
            ["pulumi"] = Declared("pulumi"),
            ["shellcheck"] = Declared("shellcheck"),
            ["skopeo"] = Declared("skopeo"),
            ["snyk"] = Declared("snyk"),
            ["sonar-scanner"] = new()
            {
                CommandName = "sonar-scanner",
                SupportedVersion = "8.0.1.6346",
                InstallationUrl =
                    "https://docs.sonarsource.com/sonarqube-server/analyzing-source-code/scanners/sonarscanner",
                InstallationNotes =
                    "The generator workflow downloads the Linux x64 SonarScanner CLI distribution.",
            },
            ["syft"] = new()
            {
                CommandName = "syft",
                InstallationUrl = "https://oss.anchore.com/docs/installation/syft/",
            },
            ["terraform"] = new()
            {
                CommandName = "terraform",
                InstallationUrl = "https://developer.hashicorp.com/terraform/install",
            },
            ["trivy"] = Declared("trivy"),
            ["vault"] = Declared("vault"),
            ["winget"] = Declared("winget"),
            ["yarn"] = Declared("yarn"),
            ["yq"] = Declared("yq"),
        };

    public static CliToolDefinition Apply(CliToolDefinition tool)
    {
        if (tool.ExecutablePrerequisite is not null
            || !string.IsNullOrWhiteSpace(tool.ExecutablePrerequisiteMetadataExemption))
        {
            return tool;
        }

        return Prerequisites.TryGetValue(tool.ToolName, out var prerequisite)
            ? tool with { ExecutablePrerequisite = prerequisite }
            : tool;
    }

    public static CliToolDefinition PrepareForGeneration(CliToolDefinition tool)
    {
        var preparedTool = Apply(tool);
        Validate(preparedTool);
        return preparedTool;
    }

    private static void Validate(CliToolDefinition tool)
    {
        if (tool.ExecutablePrerequisite is null)
        {
            if (!string.IsNullOrWhiteSpace(tool.ExecutablePrerequisiteMetadataExemption))
            {
                return;
            }

            throw new InvalidOperationException(
                $"CLI tool '{tool.ToolName}' has no executable prerequisite metadata or explicit exemption.");
        }

        if (string.IsNullOrWhiteSpace(tool.ExecutablePrerequisite.CommandName))
        {
            throw new InvalidOperationException(
                $"CLI tool '{tool.ToolName}' has executable prerequisite metadata with no command name.");
        }

        if (!string.IsNullOrWhiteSpace(tool.ExecutablePrerequisite.InstallationUrl)
            && (!Uri.TryCreate(
                    tool.ExecutablePrerequisite.InstallationUrl,
                    UriKind.Absolute,
                    out var installationUri)
                || installationUri.Scheme != Uri.UriSchemeHttps))
        {
            throw new InvalidOperationException(
                $"CLI tool '{tool.ToolName}' has an invalid HTTPS installation URL.");
        }
    }

    private static CliExecutablePrerequisite Declared(string commandName) => new()
    {
        CommandName = commandName,
        InstallationNotes = GenericInstallationNotes,
    };
}

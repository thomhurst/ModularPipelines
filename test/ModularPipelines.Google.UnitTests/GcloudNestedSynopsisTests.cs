using System.ComponentModel.DataAnnotations;
using ModularPipelines.Google.Options;

namespace ModularPipelines.Google.UnitTests;

public class GcloudNestedSynopsisTests
{
    [Test]
    [Arguments("cpu-pools", true)]
    [Arguments("none", false)]
    [Arguments("cpu", true)]
    [Arguments("pools", true)]
    [Arguments("clear", true)]
    [Arguments("cpu-clear", true)]
    [Arguments("resources", true)]
    [Arguments("conflict", false)]
    [Arguments("cpu-conflict", false)]
    public async Task Kafka_Updates_Preserve_Nonexclusive_Settings_And_Exclusive_Ca_Pools(string selection, bool valid)
    {
        var options = new GcloudManagedKafkaClustersUpdateOptions("cluster")
        {
            Cpu = selection is "cpu-pools" or "cpu" or "cpu-clear" or "resources" or "cpu-conflict" ? "3" : null,
            Memory = selection == "resources" ? "3Gi" : null,
            MtlsCaPools = selection is "cpu-pools" or "pools" or "conflict" or "cpu-conflict" ? ["projects/project/locations/location/caPools/pool"] : null,
            ClearMtlsCaPools = selection is "clear" or "cpu-clear" or "conflict" or "cpu-conflict" ? true : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("none", false)]
    [Arguments("description", true)]
    [Arguments("labels", true)]
    [Arguments("combined", true)]
    [Arguments("config", true)]
    [Arguments("config-only", false)]
    [Arguments("mask-only", false)]
    [Arguments("mixed", false)]
    public async Task Cluster_Director_Updates_Keep_Granular_Flags_Optional(string selection, bool valid)
    {
        var options = new GcloudClusterDirectorClustersUpdateOptions("cluster")
        {
            Description = selection is "description" or "combined" or "mixed" ? "Updated cluster" : null,
            RemoveLabels = selection is "labels" or "combined" ? ["old-label"] : null,
            Config = selection is "config" or "config-only" or "mixed" ? "cluster.json" : null,
            UpdateMask = selection is "config" or "mask-only" or "mixed" ? "description" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("none", false)]
    [Arguments("workers", true)]
    [Arguments("scheduler", true)]
    [Arguments("combined", true)]
    [Arguments("nodes", true)]
    [Arguments("mixed", false)]
    [Arguments("window", true)]
    [Arguments("window-only", false)]
    public async Task Composer_Updates_Keep_Resource_Settings_Optional(string selection, bool valid)
    {
        var options = new GcloudComposerEnvironmentsUpdateOptions("environment")
        {
            MaxWorkers = selection is "workers" or "combined" or "mixed" ? "2" : null,
            SchedulerCount = selection is "scheduler" or "combined" ? 1 : null,
            NodeCount = selection is "nodes" or "mixed" ? 3 : null,
            MaintenanceWindowStart = selection is "window" or "window-only" ? "2026-09-22T01:00:00Z" : null,
            MaintenanceWindowEnd = selection == "window" ? "2026-09-22T05:00:00Z" : null,
            MaintenanceWindowRecurrence = selection == "window" ? "FREQ=WEEKLY;BYDAY=TU" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("file", true)]
    [Arguments("enable", true)]
    [Arguments("enable-only", false)]
    [Arguments("cpu", true)]
    [Arguments("memory", true)]
    [Arguments("enable-cpu", false)]
    [Arguments("enable-memory", false)]
    [Arguments("limits", true)]
    [Arguments("mixed", false)]
    public async Task Container_Autoprovisioning_Keeps_Settings_Optional(string selection, bool valid)
    {
        var options = new GcloudContainerClustersUpdateOptions("cluster")
        {
            AutoprovisioningConfigFile = selection is "file" or "enable" or "mixed" ? "autoprovisioning.yaml" : null,
            EnableAutoprovisioning = selection is "enable" or "enable-only" or "enable-cpu" or "enable-memory" or "limits" ? true : null,
            MaxCpu = selection is "cpu" or "enable-cpu" or "limits" ? "8" : null,
            MaxMemory = selection is "memory" or "enable-memory" or "limits" ? "16" : null,
            AutoprovisioningMinCpuPlatform = selection == "mixed" ? "Intel Ice Lake" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("none", false)]
    [Arguments("config", true)]
    [Arguments("branch", true)]
    [Arguments("tag", true)]
    [Arguments("build", true)]
    [Arguments("description", true)]
    [Arguments("combined", true)]
    [Arguments("mixed-config", false)]
    [Arguments("both-patterns", false)]
    [Arguments("both-builds", false)]
    public async Task Build_Trigger_Updates_Preserve_Documented_Nested_Choices(string selection, bool valid)
    {
        GcloudBuildsTriggersUpdateGithubOptions options = selection switch
        {
            "none" => new("trigger"),
            "config" => new("trigger") { TriggerConfig = "trigger.yaml" },
            "branch" => new("trigger") { BranchPattern = ".*" },
            "tag" => new("trigger") { TagPattern = "v.*" },
            "build" => new("trigger") { BuildConfig = "cloudbuild.yaml" },
            "description" => new("trigger") { Description = "Updated trigger" },
            "combined" => new("trigger") { BranchPattern = ".*", BuildConfig = "cloudbuild.yaml", Description = "Updated trigger" },
            "mixed-config" => new("trigger") { TriggerConfig = "trigger.yaml", BranchPattern = ".*" },
            "both-patterns" => new("trigger") { BranchPattern = ".*", TagPattern = "v.*" },
            "both-builds" => new("trigger") { BuildConfig = "cloudbuild.yaml", InlineConfig = "inline.yaml" },
            _ => throw new ArgumentOutOfRangeException(nameof(selection)),
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("bucket-manifest", true)]
    [Arguments("bucket-prefix", true)]
    [Arguments("list-prefix", true)]
    [Arguments("bucket-only", false)]
    [Arguments("filter-only", false)]
    [Arguments("both-buckets", false)]
    [Arguments("both-filters", false)]
    [Arguments("dry-run", true)]
    [Arguments("insights", true)]
    [Arguments("insights-only", false)]
    [Arguments("mixed", false)]
    public async Task Storage_Source_Branches_Keep_Bucket_Selectors_And_Filters_Together(string source, bool valid)
    {
        GcloudStorageBatchOperationsJobsCreateOptions options = source switch
        {
            "bucket-manifest" => new("job") { DeleteObject = true, Bucket = "bucket", ManifestLocation = "gs://bucket/manifest.csv" },
            "bucket-prefix" => new("job") { DeleteObject = true, Bucket = "bucket", IncludedObjectPrefixes = "prefix" },
            "list-prefix" => new("job") { DeleteObject = true, BucketList = ["bucket"], IncludedObjectPrefixes = "prefix" },
            "bucket-only" => new("job") { DeleteObject = true, Bucket = "bucket" },
            "filter-only" => new("job") { DeleteObject = true, ManifestLocation = "gs://bucket/manifest.csv" },
            "both-buckets" => new("job") { DeleteObject = true, Bucket = "bucket", BucketList = ["bucket"], ManifestLocation = "gs://bucket/manifest.csv" },
            "both-filters" => new("job") { DeleteObject = true, Bucket = "bucket", ManifestLocation = "gs://bucket/manifest.csv", IncludedObjectPrefixes = "prefix" },
            "dry-run" => new("job") { DeleteObject = true, DryRunJobId = "dry-run-job" },
            "insights" => new("job") { DeleteObject = true, InsightsDataSetConfig = "dataset", TargetProject = "project" },
            "insights-only" => new("job") { DeleteObject = true, InsightsDataSetConfig = "dataset" },
            "mixed" => new("job") { DeleteObject = true, Bucket = "bucket", ManifestLocation = "gs://bucket/manifest.csv", DryRunJobId = "dry-run-job" },
            _ => throw new ArgumentOutOfRangeException(nameof(source)),
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("none", false)]
    [Arguments("clear-all", true)]
    [Arguments("clear-and-update", true)]
    [Arguments("clear-only", true)]
    [Arguments("update-only", true)]
    [Arguments("clear-and-file", true)]
    [Arguments("update-and-file", false)]
    [Arguments("file", true)]
    [Arguments("mixed", false)]
    public async Task Storage_Custom_Context_Alternatives_Preserve_Documented_Choices(string selection, bool valid)
    {
        var options = new GcloudStorageBatchOperationsJobsCreateOptions("job")
        {
            Bucket = "bucket",
            ManifestLocation = "gs://bucket/manifest.csv",
            ClearAllObjectCustomContexts = selection is "clear-all" or "mixed" ? true : null,
            ClearObjectCustomContexts = selection is "clear-and-update" or "clear-only" or "clear-and-file" ? ["old-key"] : null,
            UpdateObjectCustomContexts = selection is "clear-and-update" or "update-only" or "update-and-file" ? ["key=value"] : null,
            UpdateObjectCustomContextsFile = selection is "file" or "mixed" or "clear-and-file" or "update-and-file" ? "contexts.json" : null,
        };
        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }

    [Test]
    [Arguments("api-key", "", true)]
    [Arguments("two", "", true)]
    [Arguments("three", "", true)]
    [Arguments("two", "client-id", true)]
    [Arguments("two", "secret", true)]
    [Arguments("two", "token-url", true)]
    [Arguments("three", "client-id", true)]
    [Arguments("three", "secret", true)]
    [Arguments("three", "token-url", true)]
    [Arguments("three", "authorization-url", true)]
    [Arguments("three", "continue-uri", true)]
    [Arguments("three", "pkce", true)]
    public async Task Agent_Identity_Oauth_Preserves_Optional_Branch_Members(string branch, string missing, bool valid)
    {
        GcloudAgentIdentityAuthProvidersCreateOptions options = branch switch
        {
            "api-key" => new("provider") { ApiKey = "api-key" },
            "two" => new("provider")
            {
                TwoLeggedOauthClientId = "client",
                TwoLeggedOauthClientSecret = "secret",
                TwoLeggedOauthTokenUrl = "https://example.com/token",
            },
            "three" => new("provider")
            {
                ThreeLeggedOauthClientId = "client",
                ThreeLeggedOauthClientSecret = "secret",
                ThreeLeggedOauthTokenUrl = "https://example.com/token",
                ThreeLeggedOauthAuthorizationUrl = "https://example.com/authorize",
                ThreeLeggedOauthDefaultContinueUri = "https://example.com/continue",
                ThreeLeggedOauthEnablePkce = true,
            },
            _ => throw new ArgumentOutOfRangeException(nameof(branch)),
        };
        switch (missing)
        {
            case "":
                break;
            case "client-id":
                options.TwoLeggedOauthClientId = null;
                options.ThreeLeggedOauthClientId = null;
                break;
            case "secret":
                options.TwoLeggedOauthClientSecret = null;
                options.ThreeLeggedOauthClientSecret = null;
                break;
            case "token-url":
                options.TwoLeggedOauthTokenUrl = null;
                options.ThreeLeggedOauthTokenUrl = null;
                break;
            case "authorization-url":
                options.ThreeLeggedOauthAuthorizationUrl = null;
                break;
            case "continue-uri":
                options.ThreeLeggedOauthDefaultContinueUri = null;
                break;
            case "pkce":
                options.ThreeLeggedOauthEnablePkce = null;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(missing));
        }

        var errors = new List<ValidationResult>();
        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid);
    }
}

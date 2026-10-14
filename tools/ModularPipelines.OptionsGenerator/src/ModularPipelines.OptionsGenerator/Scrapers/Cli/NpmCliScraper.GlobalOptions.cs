using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Scrapers.Cli;

public partial class NpmCliScraper
{
    protected override bool GlobalOptionsBeforeSubcommands => false;

    // npm's terse root help lists commands, not config definitions. These shared
    // execution/logging/transport settings come from npm 11's config reference.
    // Install, publish, script and workspace settings remain command-local.
    protected override IReadOnlyList<CliOptionDefinition> SupplementalGlobalOptions =>
    [
        ConfigOption("prefix", "Prefix", "string?", "Run non-global commands in this directory; also sets the global installation prefix.", shortForm: "-C"),
        ConfigOption("userconfig", "Userconfig", "string?", "Read user configuration from this file."),
        ConfigOption("globalconfig", "Globalconfig", "string?", "Read global configuration from this file."),
        ConfigOption("cache", "Cache", "string?", "Directory used for npm's cache."),
        ConfigOption("loglevel", "Loglevel", "string?", "Log verbosity: silent, error, warn, notice, http, info, verbose or silly."),
        ConfigOption("logs-dir", "LogsDir", "string?", "Directory used for npm's log files."),
        ConfigOption("logs-max", "LogsMax", "int?", "Maximum number of log files to retain; zero disables log files."),
        ConfigOption("heading", "Heading", "string?", "Prefix used for debugging log output."),
        ConfigOption("timing", "Timing", "bool?", "Write timing information for this invocation."),
        ConfigOption("color", "Color", "string?", "Color output policy: true, false or always."),
        ConfigOption("unicode", "Unicode", "bool?", "Use Unicode characters instead of ASCII in output."),
        ConfigOption("progress", "Progress", "bool?", "Display progress during time-intensive operations."),
        ConfigOption("registry", "Registry", "string?", "Base URL of the package registry."),
        ConfigOption("ca", "Ca", "IEnumerable<string>?", "Trusted certificate authority certificates in PEM format.", repeatable: true),
        ConfigOption("cafile", "Cafile", "string?", "File containing trusted certificate authority certificates."),
        ConfigOption("strict-ssl", "StrictSsl", "bool?", "Validate TLS certificates for registry requests."),
        ConfigOption("https-proxy", "HttpsProxy", "string?", "Proxy URL for HTTPS requests; may contain credentials.", secret: true),
        ConfigOption("proxy", "Proxy", "string?", "Proxy URL for HTTP requests; may contain credentials.", secret: true),
        ConfigOption("noproxy", "Noproxy", "IEnumerable<string>?", "Hosts that bypass the proxy.", repeatable: true),
        ConfigOption("offline", "Offline", "bool?", "Use the cache without network requests."),
        ConfigOption("prefer-offline", "PreferOffline", "bool?", "Skip cache staleness checks but fetch missing data."),
        ConfigOption("prefer-online", "PreferOnline", "bool?", "Check cached data for freshness immediately."),
        ConfigOption("fetch-retries", "FetchRetries", "int?", "Number of retries for registry reads."),
        ConfigOption("fetch-retry-factor", "FetchRetryFactor", "double?", "Exponential factor used for retry delays."),
        ConfigOption("fetch-retry-mintimeout", "FetchRetryMintimeout", "int?", "Minimum retry delay in milliseconds."),
        ConfigOption("fetch-retry-maxtimeout", "FetchRetryMaxtimeout", "int?", "Maximum retry delay in milliseconds."),
        ConfigOption("fetch-timeout", "FetchTimeout", "int?", "Maximum request duration in milliseconds."),
    ];

    private static CliOptionDefinition ConfigOption(string name, string propertyName, string type, string description,
        string? shortForm = null, bool repeatable = false, bool secret = false) => new()
        {
            SwitchName = "--" + name,
            ShortForm = shortForm,
            PropertyName = propertyName,
            CSharpType = type,
            Description = description,
            DocumentationUrl = "https://docs.npmjs.com/cli/v11/using-npm/config#" + name,
            // Config booleans accept explicit false; color additionally accepts 'always'.
            IsFlag = false,
            ValueSeparator = "=",
            AcceptsMultipleValues = repeatable,
            IsNumeric = type is "int?" or "double?",
            IsSecret = secret,
        };
}

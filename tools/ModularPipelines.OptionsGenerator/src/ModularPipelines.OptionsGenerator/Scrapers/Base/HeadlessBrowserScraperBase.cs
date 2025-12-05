using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace ModularPipelines.OptionsGenerator.Scrapers.Base;

/// <summary>
/// Base class for scrapers that require JavaScript rendering (React, Vue, etc.).
/// Uses Playwright for headless browser automation.
/// </summary>
public abstract class HeadlessBrowserScraperBase : CliDocumentationScraperBase
{
    private readonly Lazy<Task<IPlaywright>> _playwright;
    private IBrowser? _browser;

    protected HeadlessBrowserScraperBase(HttpClient httpClient, ILogger logger)
        : base(httpClient, logger)
    {
        _playwright = new Lazy<Task<IPlaywright>>(() => Playwright.CreateAsync());
    }

    /// <summary>
    /// Gets or creates a browser instance.
    /// </summary>
    protected async Task<IBrowser> GetBrowserAsync()
    {
        if (_browser is not null)
            return _browser;

        var playwright = await _playwright.Value;
        _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });

        return _browser;
    }

    /// <summary>
    /// Fetches and waits for JavaScript-rendered content.
    /// </summary>
    protected async Task<AngleSharp.Dom.IDocument> FetchRenderedHtmlAsync(
        string url,
        string waitForSelector,
        CancellationToken cancellationToken)
    {
        Logger.LogDebug("Fetching rendered content from {Url}", url);

        var browser = await GetBrowserAsync();
        var page = await browser.NewPageAsync();

        try
        {
            await page.GotoAsync(url, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });

            await page.WaitForSelectorAsync(waitForSelector, new PageWaitForSelectorOptions
            {
                Timeout = 30000 // 30 seconds
            });

            var content = await page.ContentAsync();
            return await HtmlParser.ParseDocumentAsync(content, cancellationToken);
        }
        finally
        {
            await page.CloseAsync();
        }
    }

    /// <summary>
    /// Disposes the browser resources.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.CloseAsync();
            _browser = null;
        }

        if (_playwright.IsValueCreated)
        {
            var playwright = await _playwright.Value;
            playwright.Dispose();
        }
    }
}

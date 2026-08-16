import Link from '@docusaurus/Link';
import Layout from '@theme/Layout';

import styles from './index.module.css';

function ArrowIcon(): JSX.Element {
  return (
    <svg viewBox="0 0 16 16" aria-hidden="true">
      <path d="M2.5 8h10M8.5 4l4 4-4 4" />
    </svg>
  );
}

function GithubIcon(): JSX.Element {
  return (
    <svg viewBox="0 0 24 24" aria-hidden="true">
      <path d="M12 2a10 10 0 0 0-3.16 19.49c.5.09.68-.22.68-.48v-1.86c-2.78.6-3.37-1.18-3.37-1.18-.45-1.16-1.11-1.47-1.11-1.47-.91-.62.07-.61.07-.61 1 .07 1.53 1.03 1.53 1.03.9 1.53 2.35 1.09 2.92.83.09-.65.35-1.09.64-1.34-2.22-.25-4.55-1.11-4.55-4.94 0-1.09.39-1.98 1.03-2.68-.1-.25-.45-1.27.1-2.64 0 0 .84-.27 2.75 1.02A9.57 9.57 0 0 1 12 6.84a9.6 9.6 0 0 1 2.5.34c1.9-1.29 2.74-1.02 2.74-1.02.55 1.37.2 2.39.1 2.64.64.7 1.03 1.59 1.03 2.68 0 3.84-2.34 4.68-4.57 4.93.36.31.68.92.68 1.86v2.75c0 .27.18.58.69.48A10 10 0 0 0 12 2Z" />
    </svg>
  );
}

function PipelineVisual(): JSX.Element {
  return (
    <div className={styles.visualWrap} aria-label="A build pipeline dependency graph running tasks in parallel">
      <div className={styles.visualLabel}>LIVE ORCHESTRATION</div>
      <div className={styles.pipelineWindow}>
        <div className={styles.windowBar}>
          <div className={styles.windowDots} aria-hidden="true"><i /><i /><i /></div>
          <span>release-pipeline</span>
          <span className={styles.running}><i /> running</span>
        </div>
        <div className={styles.pipelineBody}>
          <div className={styles.pipelineMeta}>
            <span>STATUS <strong>active</strong></span>
            <span>BRANCH <strong>main</strong></span>
            <span>MODE <strong>parallel</strong></span>
          </div>
          <div className={styles.graph}>
            <div className={`${styles.node} ${styles.nodeRestore}`}>
              <span className={styles.nodeIcon}>✓</span>
              <span><small>complete</small>Restore</span>
            </div>
            <div className={`${styles.connector} ${styles.connectorFirst}`}><i /></div>
            <div className={styles.branchLines} aria-hidden="true"><i /><i /><i /></div>
            <div className={`${styles.node} ${styles.nodeTest}`}>
              <span className={styles.nodeIcon}>✓</span>
              <span><small>complete</small>Test</span>
            </div>
            <div className={`${styles.node} ${styles.nodeAnalyse}`}>
              <span className={styles.nodeSpinner} />
              <span><small>running</small>Analyse</span>
            </div>
            <div className={`${styles.node} ${styles.nodePackage}`}>
              <span className={styles.nodeSpinner} />
              <span><small>running</small>Package</span>
            </div>
            <div className={styles.mergeLines} aria-hidden="true"><i /><i /><i /></div>
            <div className={`${styles.connector} ${styles.connectorLast}`}><i /></div>
            <div className={`${styles.node} ${styles.nodePublish}`}>
              <span className={styles.waitingDot} />
              <span><small>waiting</small>Publish</span>
            </div>
          </div>
          <div className={styles.consoleLine}>
            <span>&gt;</span> independent modules executing <b>concurrently</b><i />
          </div>
        </div>
      </div>
    </div>
  );
}

function FeatureIcon({name}: {name: 'nodes' | 'code' | 'terminal'}): JSX.Element {
  if (name === 'nodes') {
    return <svg viewBox="0 0 32 32" aria-hidden="true"><circle cx="7" cy="7" r="3" /><circle cx="25" cy="16" r="3" /><circle cx="7" cy="25" r="3" /><path d="M10 8.5 22 14M10 23.5 22 18" /></svg>;
  }
  if (name === 'code') {
    return <svg viewBox="0 0 32 32" aria-hidden="true"><path d="m11 8-7 8 7 8M21 8l7 8-7 8M19 5l-6 22" /></svg>;
  }
  return <svg viewBox="0 0 32 32" aria-hidden="true"><rect x="3" y="5" width="26" height="22" rx="3" /><path d="m8 12 5 4-5 4M16 21h7" /></svg>;
}

export default function Home(): JSX.Element {
  return (
    <Layout
      title="Build pipelines that think in parallel"
      description="Modular Pipelines is a C# framework for building strongly typed, modular and automatically orchestrated CI/CD pipelines.">
      <header className={styles.hero}>
        <div className={styles.heroGrid} />
        <div className={`container ${styles.heroInner}`}>
          <div className={styles.heroCopy}>
            <div className={styles.eyebrow}><span>C# NATIVE</span> CI/CD ORCHESTRATION</div>
            <h1>Build pipelines<br />that <em>think in parallel.</em></h1>
            <p className={styles.heroDescription}>
              Break delivery work into focused C# modules. Declare what depends on what. Modular Pipelines works out what can run now and what needs to wait.
            </p>
            <div className={styles.heroActions}>
              <Link className={styles.primaryButton} to="/docs/next/getting-started">
                Start building <ArrowIcon />
              </Link>
              <Link className={styles.secondaryButton} href="https://github.com/thomhurst/ModularPipelines">
                <GithubIcon /> Explore the source
              </Link>
            </div>
            <div className={styles.proofRow}>
              <span><i /> Strongly typed</span>
              <span><i /> CI agnostic</span>
              <span><i /> Built on .NET</span>
            </div>
          </div>
          <PipelineVisual />
        </div>
      </header>

      <main>
        <section className={styles.manifesto}>
          <div className="container">
            <div className={styles.sectionIntro}>
              <div>
                <span className={styles.kicker}>YOUR CODE. YOUR RULES.</span>
                <h2>Stop scripting.<br /><em>Start engineering.</em></h2>
              </div>
              <p>
                Your delivery logic deserves the same structure, tooling, and confidence as the software it ships. Write ordinary C# and let the dependency graph handle the choreography.
              </p>
            </div>
            <div className={styles.featureGrid}>
              <article className={`${styles.feature} ${styles.featureWide}`}>
                <span className={styles.featureIcon}><FeatureIcon name="nodes" /></span>
                <div>
                  <span className={styles.featureTag}>ORCHESTRATION</span>
                  <h3>Declare dependencies.<br />Unlock concurrency.</h3>
                  <p>Modules run the moment their dependencies are ready. Independent work fans out automatically—no hand-built job graph required.</p>
                </div>
                <div className={styles.miniGraph} aria-hidden="true">
                  <span>A</span><i /><span>B</span><i /><span>C</span>
                  <b /><span>D</span>
                </div>
              </article>
              <article className={`${styles.feature} ${styles.featureAccent}`}>
                <span className={styles.featureIcon}><FeatureIcon name="code" /></span>
                <span className={styles.featureTag}>FAMILIAR</span>
                <h3>Real C#. Real tooling.</h3>
                <p>Use generics, dependency injection, configuration, testing, and every library in the .NET ecosystem.</p>
                <code>Module&lt;TResult&gt;</code>
              </article>
              <article className={styles.feature}>
                <span className={styles.featureIcon}><FeatureIcon name="terminal" /></span>
                <span className={styles.featureTag}>PORTABLE</span>
                <h3>One pipeline.<br />Run it anywhere.</h3>
                <p>GitHub Actions, Azure Pipelines, TeamCity, or your laptop. If it runs .NET, it runs your pipeline.</p>
                <div className={styles.providerList}><span>GH</span><span>AZ</span><span>TC</span><span>_</span></div>
              </article>
            </div>
          </div>
        </section>

        <section className={styles.codeSection}>
          <div className={`container ${styles.codeSectionInner}`}>
            <div className={styles.codeCopy}>
              <span className={styles.kicker}>FROM ZERO TO ORCHESTRATED</span>
              <h2>Dependencies are<br /><em>part of the type system.</em></h2>
              <p>Make the relationship explicit and Modular Pipelines takes care of ordering, parallelization, results, and cancellation.</p>
              <Link className={styles.textLink} to="/docs/how-to/defining-modules">Learn how modules work <ArrowIcon /></Link>
            </div>
            <div className={styles.codeWindow}>
              <div className={styles.codeToolbar}>
                <div><i /><i /><i /></div>
                <span>RunTestsModule.cs</span>
                <b>C#</b>
              </div>
              <pre><code><span className={styles.keyword}>[DependsOn</span>&lt;RestoreModule&gt;<span className={styles.keyword}>]</span>{'\n'}<span className={styles.keyword}>public sealed class</span> <span className={styles.type}>RunTestsModule</span>{'\n'}    : Module&lt;TestResult&gt;{'\n'}{'{'}{'\n'}    <span className={styles.keyword}>protected override async Task</span>&lt;TestResult&gt;{'\n'}        ExecuteAsync(ModuleContext context,{'\n'}            CancellationToken cancellationToken){'\n'}    {'{'}{'\n'}        <span className={styles.comment}>// Your pipeline is just C#.</span>{'\n'}        <span className={styles.keyword}>return await</span> context.DotNet().Test(...);{'\n'}    {'}'}{'\n'}{'}'}</code></pre>
              <div className={styles.codeFooter}><span><i /> build succeeded</span><span>UTF-8</span></div>
            </div>
          </div>
        </section>

        <section className={styles.finalCta}>
          <div className="container">
            <div className={styles.ctaPanel}>
              <div className={styles.ctaSignal} aria-hidden="true"><span /><span /><span /></div>
              <div><span className={styles.kicker}>READY WHEN YOU ARE</span><h2>Give your pipeline a proper architecture.</h2></div>
              <Link className={styles.primaryButton} to="/docs/next/getting-started">Read the quickstart <ArrowIcon /></Link>
            </div>
          </div>
        </section>
      </main>
    </Layout>
  );
}

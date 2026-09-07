import {useState, type JSX} from 'react';
import Link from '@docusaurus/Link';
import Layout from '@theme/Layout';

import styles from './index.module.css';

const modules = {
  restore: {
    name: 'Restore', dependencies: [],
    description: 'Start with the packages. Restore has no dependencies, so it can run as soon as the pipeline starts.',
  },
  build: {
    name: 'Build', dependencies: ['Restore'],
    description: 'Compile once the packages are ready. Build waits for Restore to finish successfully.',
  },
  test: {
    name: 'Test', dependencies: ['Build'],
    description: 'Check the build. Test can run alongside Pack because neither module depends on the other.',
  },
  pack: {
    name: 'Pack', dependencies: ['Build'],
    description: 'Prepare the package. Pack can run alongside Test as soon as their shared Build dependency succeeds.',
  },
  publish: {
    name: 'Publish', dependencies: ['Test', 'Pack'],
    description: 'Ship when both branches are ready. Publish waits for Test and Pack to finish successfully.',
  },
} satisfies Record<string, {name: string; dependencies: string[]; description: string}>;

type ModuleId = keyof typeof modules;
const moduleIds = Object.keys(modules) as ModuleId[];

function DependencyMap(): JSX.Element {
  const [selected, setSelected] = useState<ModuleId>('pack');
  const module = modules[selected];

  return (
    <section className={styles.workbench} aria-labelledby="map-title">
      <div className={styles.mapPanel}>
        <div className={styles.mapHeading}>
          <h2 id="map-title">A release, connected.</h2>
          <span>Explore the modules</span>
        </div>
        <div className={styles.graph} role="group" aria-label="Release pipeline modules">
          <svg className={styles.desktopRoutes} viewBox="0 0 720 300" preserveAspectRatio="none" aria-hidden="true">
            <path className={styles.sharedRoute} d="M86 150 H259" />
            <path className={styles.testRoute} d="M259 150 H307 Q329 150 329 128 V97 Q329 75 351 75 H447" />
            <path className={styles.packRoute} d="M259 150 H307 Q329 150 329 172 V203 Q329 225 351 225 H447" />
            <path className={styles.testRoute} d="M447 75 H524 Q546 75 546 97 V128 Q546 150 568 150 H641" />
            <path className={styles.packRoute} d="M447 225 H524 Q546 225 546 203 V172 Q546 150 568 150 H641" />
          </svg>
          <svg className={styles.mobileRoutes} viewBox="0 0 320 360" preserveAspectRatio="none" aria-hidden="true">
            <path className={styles.sharedRoute} d="M160 36 V119" />
            <path className={styles.testRoute} d="M160 119 V145 Q160 163 142 163 H98 Q80 163 80 181 V212" />
            <path className={styles.packRoute} d="M160 119 V145 Q160 163 178 163 H222 Q240 163 240 181 V212" />
            <path className={styles.testRoute} d="M80 212 V250 Q80 268 98 268 H142 Q160 268 160 286 V310" />
            <path className={styles.packRoute} d="M240 212 V250 Q240 268 222 268 H178 Q160 268 160 286 V310" />
          </svg>
          {moduleIds.map(id => (
            <button key={id} type="button"
              className={`${styles.module} ${styles[id]}`}
              aria-pressed={selected === id} aria-controls="module-detail"
              onClick={() => setSelected(id)}>
              <span className={styles.socket} aria-hidden="true"><span /></span>
              <span className={styles.moduleName}>{modules[id].name}</span>
            </button>
          ))}
          <span className={styles.parallelNote}>Independent work</span>
        </div>
        <p className={styles.mapCaption}>Follow the dependencies. The parallelism follows.</p>
      </div>
      <div id="module-detail" className={styles.moduleDetail} aria-live="polite" aria-atomic="true">
        <div className={styles.fileName}><span aria-hidden="true">{'{ }'}</span> {module.name}Module.cs</div>
        <div className={styles.detailBody}>
          <span className={styles.detailLabel}>Configuration excerpt</span>
          <pre aria-label={`${module.name} module configuration`}><code>
            <span className={styles.keyword}>protected override void</span>{'\n'}
            Configure({'\n'}
            {'    '}<span className={styles.codeType}>ModuleConfigurationBuilder</span> module){'\n'}
            {module.dependencies.length > 0 ? <>
              {'    => module'}
              {module.dependencies.map((dependency, index) => (
                <span key={dependency}>{'\n        '}.DependsOn&lt;<span className={styles.codeType}>{dependency}Module</span>&gt;(){index === module.dependencies.length - 1 ? ';' : ''}</span>
              ))}
            </> : <>{'{'}{'\n    '}<span className={styles.codeComment}>// No dependencies to configure.</span>{'\n'}{'}'}</>}
          </code></pre>
          <h3>{module.dependencies.length === 0 ? 'Ready from the start.' : `After ${module.dependencies.join(' and ')}.`}</h3>
          <p>{module.description}</p>
          <Link to="/docs/next/how-to/execution-and-dependencies">Read about dependencies</Link>
        </div>
      </div>
    </section>
  );
}

export default function Home(): JSX.Element {
  return (
    <Layout title="Your pipeline. Piece by piece."
      description="Build your delivery pipeline from focused C# modules. Declare dependencies and let ModularPipelines run independent work in parallel.">
      <main className={styles.home}>
        <div className={styles.pageWidth}>
          <header className={styles.intro}>
            <h1>Your pipeline.<br />Piece by piece.</h1>
            <div className={styles.introCopy}>
              <p>Build, test, and ship with focused C# modules. You declare the dependencies. ModularPipelines connects the work and runs it in parallel.</p>
              <div className={styles.actions}>
                <Link className={styles.primaryButton} to="/docs/next/getting-started">Build your first pipeline</Link>
                <Link className={styles.sourceLink} href="https://github.com/thomhurst/ModularPipelines">View source on GitHub</Link>
              </div>
            </div>
          </header>
          <DependencyMap />
          <section className={styles.handbook} aria-labelledby="handbook-title">
            <div className={styles.handbookIntro}>
              <h2 id="handbook-title">Small modules.<br />Room to build.</h2>
              <p>Keep the tools you know.<br />Give delivery its own structure.</p>
            </div>
            <div className={styles.readingList}>
              <Link className={styles.readingLink} to="/docs/next/how-to/defining-modules">
                <span className={`${styles.readingSymbol} ${styles.csharpSymbol}`} aria-hidden="true">{'{ }'}</span>
                <div><h3>C# all the way down</h3><p>Types, dependency injection, and your favourite .NET libraries. Each module is an ordinary C# class.</p><span>Write a module</span></div>
              </Link>
              <Link className={styles.readingLink} to="/docs/next/how-to/parallelization">
                <span className={`${styles.readingSymbol} ${styles.parallelSymbol}`} aria-hidden="true">Ⅱ</span>
                <div><h3>Let independent work overlap</h3><p>Dependencies set the order. The framework schedules modules as their prerequisites complete.</p><span>Understand parallel execution</span></div>
              </Link>
              <Link className={styles.readingLink} to="/docs/next/getting-started">
                <span className={`${styles.readingSymbol} ${styles.runSymbol}`} aria-hidden="true">&gt;_</span>
                <div><h3>From your laptop to CI</h3><p>Your pipeline is a .NET application. Run the same code locally and on your build server.</p><span>Set up your pipeline</span></div>
              </Link>
            </div>
          </section>
          <section className={styles.quickstart} aria-labelledby="start-title">
            <div><h2 id="start-title">Start with one module.</h2><p>The project template gives you the pieces for your first pipeline.</p></div>
            <div className={styles.install}><span>Install the project template</span><code>dotnet new install ModularPipelines.Templates</code><Link to="/docs/next/getting-started">Continue with the quickstart</Link></div>
          </section>
        </div>
      </main>
    </Layout>
  );
}

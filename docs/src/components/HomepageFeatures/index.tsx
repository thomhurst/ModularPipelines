import clsx from 'clsx';
import Heading from '@theme/Heading';
import styles from './styles.module.css';

type FeatureItem = {
  title: string;
  Svg: React.ComponentType<React.ComponentProps<'svg'>>;
  description: JSX.Element;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'Familiar',
    Svg: require('@site/static/img/undraw_docusaurus_mountain.svg').default,
    description: (
      <>
        If you can write your app in .NET, why not your pipelines too? Use language and libraries your already know.
        Set up follows the conventional .NET host setup you're used to, with `IConfiguration` and `IServiceProvider` at its core.
      </>
    ),
  },
  {
    title: 'Automatic Orchestration',
    Svg: require('@site/static/img/undraw_docusaurus_tree.svg').default,
    description: (
      <>
        Modular Pipelines handles parallelization and depdendency management for you. Simply tell it if something depends on something else and Modular Pipelines will run it in parallel, or wait for its dependencies, automatically.
      </>
    ),
  },
  {
    title: 'Agnostic',
    Svg: require('@site/static/img/undraw_docusaurus_react.svg').default,
    description: (
      <>
        Modular Pipelines doesn't care what build agent or system you use. You just need the .NET SDK installed and just run your .NET pipeline with a simple `dotnet run`
      </>
    ),
  },
];

function Feature({ title, Svg, description }: FeatureItem) {
  return (
    <div className={clsx('col col--4')}>
      <div className="text--center">
        <Svg className={styles.featureSvg} role="img" />
      </div>
      <div className="text--center padding-horiz--md">
        <Heading as="h3">{title}</Heading>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): JSX.Element {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}

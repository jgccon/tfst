import type { ReactNode } from "react";
import clsx from "clsx";
import Heading from "@theme/Heading";
import Translate from "@docusaurus/Translate";
import styles from "./styles.module.css";

type FeatureItem = {
  title: ReactNode;
  description: ReactNode;
  Img: string;
};

const FeatureList: FeatureItem[] = [
  {
    Img: "img/onboarding.svg",
    title: (
      <Translate
        id="homepage.feature.onboarding.title"
        description="Title for the onboarding feature"
      >
        Start with Onboarding
      </Translate>
    ),
    description: (
      <Translate
        id="homepage.feature.onboarding.description"
        description="Description for the onboarding feature"
      >
        From day one, TFST streamlines onboarding — setting roles, contracts,
        and access in motion with precision and clarity.
      </Translate>
    ),
  },
  {
    Img: "img/experience.svg",
    title: (
      <Translate
        id="homepage.feature.experience.title"
        description="Title for the experience feature"
      >
        Grow with Experience
      </Translate>
    ),
    description: (
      <Translate
        id="homepage.feature.experience.description"
        description="Description for the experience feature"
      >
        Track time, performance, and development across projects. Build skills
        aligned with ESCO and fuel real growth.
      </Translate>
    ),
  },
  {
    Img: "img/develop.svg",
    title: (
      <Translate
        id="homepage.feature.collaboration.title"
        description="Title for the collaboration feature"
      >
        Collaborate and Build
      </Translate>
    ),
    description: (
      <Translate
        id="homepage.feature.collaboration.description"
        description="Description for the collaboration feature"
      >
        Developers, managers, and contributors co-create in TFST. Modular by
        design. Transparent by default. Welcome to the full stack.
      </Translate>
    ),
  },
  {
    Img: "img/welcoming.svg",
    title: (
      <Translate
        id="homepage.feature.welcome.title"
        description="Title for the welcome feature"
      >
        Welcome to the Team
      </Translate>
    ),
    description: (
      <Translate
        id="homepage.feature.welcome.description"
        description="Description for the welcome feature"
      >
        TFST is more than a platform — it’s a shared space for people, ideas,
        and progress. Let’s build it together.
      </Translate>
    ),
  },
];

function Feature({ title, Img, description }: FeatureItem) {
  return (
    <div className={clsx("col col--6")}>
      <div className="text--center">
        <img src={Img} className={styles.featureSvg} alt="" />
      </div>
      <div className="text--center padding-horiz--md">
        <Heading as="h3">{title}</Heading>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): ReactNode {
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

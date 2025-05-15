import type { ReactNode } from "react";
import clsx from "clsx";
import Heading from "@theme/Heading";
import styles from "./styles.module.css";

type FeatureItem = {
  title: string;
   Img: string;

  description: ReactNode;
};

const FeatureList: FeatureItem[] = [
  {
    title: "Start with Onboarding",
   Img: "/img/onboarding.svg",
    description: (
      <>
        From day one, TFST streamlines onboarding — setting roles, contracts,
        and access in motion with precision and clarity.
      </>
    ),
  },
  {
    title: "Grow with Experience",
    Img: "/img/experience.svg",
    description: (
      <>
        Track time, performance, and development across projects. Build skills
        aligned with ESCO and fuel real growth.
      </>
    ),
  },
  {
    title: "Collaborate and Build",
    Img: "/img/develop.svg",
    description: (
      <>
        Developers, managers, and contributors co-create in TFST. Modular by
        design. Transparent by default. Welcome to the full stack.
      </>
    ),
  },
  {
    title: "Welcome to the Team",
    Img: "/img/welcoming.svg",
    description: (
      <>
        TFST is more than a platform — it’s a shared space for people, ideas,
        and progress. Let’s build it together.
      </>
    ),
  },
];

function Feature({ title, Img, description }: FeatureItem) {
  return (
    <div className={clsx("col col--6")}>
      <div className="text--center">
         <img src={Img} className={styles.featureSvg} alt={title} />

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

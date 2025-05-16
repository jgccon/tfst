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
    Img: "img/recognition.svg",
    title: (
      <Translate
        id="homepage.feature.recognition.title"
        description="Título de la función de incorporación"
      >
        Let your work speak for itself
      </Translate>
    ),
    description: (
      <Translate
        id="homepage.feature.recognition.description"
        description="Descripción de la función de incorporación"
      >
        Build a technical profile based on real contributions and smart contracts. Forget cover letters — your work is your voice.
      </Translate>
    ),
  },
  {
    Img: "img/talent.svg",
    title: (
      <Translate
        id="homepage.feature.talent.title"
        description="Título de la función de talento"
      >
        Hire talent without intermediaries
      </Translate>
    ),
    description: (
      <Translate
        id="homepage.feature.talent.description"
        description="Descripción de la función de talento"
      >
        Validated profiles, direct contracts, complete traceability. No inflated fees, no shady filters — just real talent.
      </Translate>
    ),
  },
  {
    Img: "img/reputation.svg",
    title: (
      <Translate
        id="homepage.feature.reputation.title"
        description="Título de la función de reputación"
      >
        Grow your reputation through real work
      </Translate>
    ),
    description: (
      <Translate
        id="homepage.feature.reputation.description"
        description="Descripción de la función de reputación"
      >
        Every logged hour, every delivered project, every review builds your reputation. Your profile becomes your proof.
      </Translate>
    ),
  },
  {
    Img: "img/control.svg",
    title: (
      <Translate
        id="homepage.feature.control.title"
        description="Título de la función de control"
      >
        Manage everything from a single place
      </Translate>
    ),
    description: (
      <Translate
        id="homepage.feature.control.description"
        description="Descripción de la función de control"
      >
       Contracts, roles, time tracking, payments — for yourself or your whole team. TFST gives you full visibility and operational control, now and as it grows.
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

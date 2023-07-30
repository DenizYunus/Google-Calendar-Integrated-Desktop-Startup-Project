import React, { useState } from "react";

const Accordion = ({ title, info, type }) => {
  const [visible, setVisible] = useState(false);
  // const [classType, setClassType] = useState(type);
  const classType = type;

  return (
    <div>
      <div
        className={
          classType === "service"
            ? "accordion-service"
            : "accordion-collaborate"
        }
      >
        <div
          className={
            classType === "service"
              ? "accordion-title-service"
              : "accordion-title-collaborate"
          }
        >
          <div>{title}</div>

          <div>
            {visible ? (
              <svg
                xmlns="http://www.w3.org/2000/svg"
                height="1em"
                viewBox="0 0 320 512"
                onClick={() => setVisible(!visible)}
              >
                <style
                  dangerouslySetInnerHTML={{
                    __html: "svg{fill:#ffffff}",
                  }}
                />
                <path d="M41.4 233.4c-12.5 12.5-12.5 32.8 0 45.3l160 160c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L109.3 256 246.6 118.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0l-160 160z" />
              </svg>
            ) : (
              <svg
                xmlns="http://www.w3.org/2000/svg"
                height="1em"
                viewBox="0 0 448 512"
                onClick={() => setVisible(!visible)}
              >
                <style
                  dangerouslySetInnerHTML={{
                    __html: "svg{fill:#ffffff}",
                  }}
                />
                <path d="M201.4 342.6c12.5 12.5 32.8 12.5 45.3 0l160-160c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L224 274.7 86.6 137.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l160 160z" />
              </svg>
            )}
          </div>
        </div>

        <div className="accordion-info">{visible && info && info}</div>
      </div>
    </div>
  );
};

export default Accordion;

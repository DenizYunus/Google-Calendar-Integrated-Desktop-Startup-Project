import React, { useState } from "react";
import Accordion from "./../components/Accordion";
import octomini from "./../utils/octomini.svg";
import pen from "./../utils/pen.svg";
import fire from "./../utils/fire.svg";
import globals from "../components/Globals";

const serviceData = [
  {
    title: "Time Awareness",
    info: "“my time runs away and I have no idea where it goes”",
    selected: false,
  },
  {
    title: "Accountability",
    info: "“need someone to check if I’m on track”",
    selected: false,
  },
  {
    title: "Improving My Work Life Balance",
    info: "“I just can’t find the right work life balance for me...”",
    selected: false,
  },
  {
    title: "Setting SMART Goals",
    info: "Specific. Measurable. Attainable. Relevant. Time-bound.",
    selected: false,
  },
  {
    title: "Sustainability",
    info:
      "“I have a productive routine, if only I could do it over and over again..”",
    selected: false,
  },
];

function ServiceSelectPage(props) {
  const handleNext = props.onNext;
  const [data, setData] = useState(serviceData);
  const [customService, setCustomService] = useState("");

  const handleAccordionClick = (index) => {
    const newData = [...data];
    newData[index].selected = !newData[index].selected;
    setData(newData);
    console.log(data + "");
  };

  return (
    <div className="content flex-center">
      <div>PROGRESS BAR</div>
      <div>
        <img src={octomini} alt="octo" width="169px" />
      </div>
      <div className="service-title">So C, how may I be of service to you?</div>
      <div className="service-subtitle">
        P.s. You can select more than 1 options, just click on them
      </div>
      <div className="accordion-area-service">
        {data.map((service, index) => (
          <div
            onClick={() => {
              handleAccordionClick(index);
            }}
            style={{
              transform:
                "scale(" + (data[index].selected ? "1.05" : "1.0") + ")",
            }}
          >
            <Accordion key={service.title} {...service} type="service" />
          </div>
        ))}
        <div className="service-input">
          <textarea
            placeholder="The stage is yours.&#10; Tell us what you need 😊"
            id="service-input"
            onChange={(event) => {
              setCustomService(event.target.value);
            }}
          ></textarea>

          <label htmlFor="service-input">
            <img src={pen} alt="pen" className="service-pen" />
          </label>
        </div>
      </div>
      <div
        className="btn service-btn flex-center"
        onClick={() => {
          globals.services = [];
          serviceData.forEach((service, index) => {
            if (service.selected) {
              globals.services.push(service.title);
            }
          });
          if (customService.trim() !== "") {
            globals.services.push(customService);
          }
          console.log(globals.services);
          handleNext();
        }}
      >
        I’m ready
        <span>
          <img
            src={fire}
            className="flex-center"
            alt="fire"
            width="28px"
            style={{ marginLeft: "8px" }}
          />
        </span>
      </div>
    </div>
  );
}

export default ServiceSelectPage;

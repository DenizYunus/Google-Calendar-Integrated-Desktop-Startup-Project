import React, { useState } from "react";
import axios from "axios";

import Accordion from "./../components/Accordion";
import pen from "./../utils/pen.svg";
import fire from "./../utils/fire.svg";
import globals from "../components/Globals";

const collaborateData = [
  {
    title: "Keep track of when I start/end things",
    info:
      "If your favourite apps haven’t met me yet, just introduce us and I’ll tell you everyhting you need to know",
    selected: false,
  },
  {
    title: "Understand my balance ratio",
    info: "For every X hours of work, you need Y hours of Only You time. ",
    selected: false,
  },
  {
    title: "Divide my work into digestible slices",
    info: "I empower you to divide your projects into slices and achieve them!",
    selected: false,
  },
  {
    title: "Resurrect my calendar",
    info:
      "Everything is everywhere. So many things going on. I feel you ~ I have just what you need",
    selected: false,
  },
  {
    title: "Stay motivated",
    info:
      "I’m distracted easily too, I octolise both of us to stay on track and achieve all our goals together",
    selected: false,
  },
];

function CollaboratePage(props) {
  const handleNext = props.onNext;
  const [data, setData] = useState(collaborateData);
  const [customCollaborate, setCustomCollaborate] = useState("");

  const handleAccordionClick = (index) => {
    const newData = [...data];
    newData[index].selected = !newData[index].selected;
    setData(newData);
  };

  return (
    <div className="content flex-center">
      <div>PROGRESS BAR</div>

      <div className="service-title">How would you like us to collaborate?</div>
      <div className="service-subtitle">
        P.s. You can select more than 1 options, just click on them
      </div>
      <div className="accordion-area-collaborate">
        {data.map((collaborate, index) => (
          <div
            onClick={() => {
              console.log(index);
              handleAccordionClick(index);
            }}
            style={{
              transform:
                "scale(" + (data[index].selected ? "1.05" : "1.0") + ")",
            }}
          >
            <Accordion
              key={collaborate.title}
              {...collaborate}
              type="collaborate"
            />
          </div>
        ))}
        <div className="collaborate-input">
          <input
            type="text"
            placeholder="The stage is yours. Tell us what you need 😊"
            id="collaborate-input"
            onChange={(event) => {
              setCustomCollaborate(event.target.value);
            }}
          />
          <label htmlFor="collaborate-input">
            <img src={pen} alt="pen" className="collaborate-pen" />
          </label>
        </div>
      </div>
      <div
        className="btn service-btn flex-center"
        onClick={() => {
          globals.collaborations = [];
          collaborateData.forEach((collaboration, index) => {
            if (collaboration.selected) {
              globals.collaborations.push(collaboration.title);
            }
          });
          if (customCollaborate.trim() !== "") {
            globals.collaborations.push(customCollaborate);
          }

          axios
            .post(
              "https://octobackendapiwindows.azurewebsites.net/api/user/complete-questions",
              {
                industry: globals.industry,
                entrepreneurField: globals.entrepreneurField,
                workingHoursInADay: globals.workingHoursInADay,
                requestedServices: globals.services,
                requestedCollaborations: globals.collaborations,
                birthday: globals.birthday,
              },
              {
                headers: {
                  Authorization: `Bearer ${globals.accessToken}`,
                },
              }
            )
            .then((response) => {
              console.log("GoogleIDToken:", globals.GoogleIDToken);
              console.log("Username:", globals.Username);
              console.log("Name:", globals.Name);
              console.log("profilePicture:", globals.profilePicture);
              console.log("accessToken:", globals.accessToken);
              console.log("refreshToken:", globals.refreshToken);
              console.log("services:", globals.services);
              console.log("industry:", globals.industry);
              console.log("birthday:", globals.birthday);
              console.log("workingHoursInADay:", globals.workingHoursInADay);
              console.log("entrepreneurField:", globals.entrepreneurField);
              console.log("collaborations:", globals.collaborations);

              handleNext();
            })
            .catch((error) => {
              console.error(error);
            });
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

export default CollaboratePage;

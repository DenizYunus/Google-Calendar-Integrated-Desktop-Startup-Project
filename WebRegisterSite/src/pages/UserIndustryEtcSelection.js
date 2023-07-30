import React, { useState } from "react";

import world from "./../utils/world.svg";
import bday from "./../utils/bday.svg";
import clock from "./../utils/clock.svg";
import fff from "./../utils/fff.svg";
import pen from "./../utils/pen.svg";

import globals from "../components/Globals";

function UserIndustryEtcSelection(props) {
  const handleNext = props.onNext;

  const industryData = [
    "AI / ML",
    "AR / VR",
    "Climate / Sustainability",
    "Community Building / Tools",
    "Consumer D2C",
    "Consumer Social",
    "Creator Economy",
    "Data Privacy",
    "Digital Health",
    "EdTech",
    "Enterprise SaaS",
    "FemTech",
    "FinTech",
    "Life Sciences",
    "Logistics",
    "Mental Health",
    "Social Gaming",
    "Other",
  ];

  const hoursData = [
    "1-2 Hours",
    "2-3 Hours",
    "3-4 Hours",
    "4-5 Hours",
    "5-6 Hours",
    "6-7 Hours",
    "7-8 Hours",
    "8-9 Hours",
    "9-10 Hours",
    "10-11 Hours",
    "11-12 Hours",
    "12+ Hours",
  ];

  const [isActiveIndustry, setIsActiveIndustry] = useState(false);
  const [selectedIndustry, setIsSelectedIndustry] = useState("Industry");
  const [isActiveHours, setIsActiveHours] = useState(false);
  const [selectedHours, setIsSelectedHours] = useState("Hours");

  return (
    <div className="content flex-center">
      <div>PROGRESS BAR</div>
      <div className="choice-person">
        <div className="choice-photo flex-center">
          <img src={globals.profilePicture ? globals.profilePicture : fff} alt="" width="280px" />
        </div>
      </div>
      <div className="your-turn">Your Turn !</div>
      <div className="choice-area flex-center">
        <div className="choice-fields">
          <div className="flex-center">
            <img
              src={world}
              alt=""
              style={{ backgroundColor: "white", borderRadius: "50%" }}
            />
          </div>
          <div className="choice-text">My industry is</div>
          <div className="flex-center">
            <div className="dropdown">
              <div
                onClick={(e) => {
                  setIsActiveIndustry(!isActiveIndustry);
                }}
                className={
                  isActiveIndustry
                    ? "dropdown-btn dropdown-true-color"
                    : "dropdown-btn dropdown-false-color"
                }
              >
                {selectedIndustry}
                <span className="flex-center">
                  {isActiveIndustry ? (
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      height="1em"
                      viewBox="0 0 320 512"
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
                    >
                      <style
                        dangerouslySetInnerHTML={{
                          __html: "svg{fill:#ffffff}",
                        }}
                      />
                      <path d="M201.4 342.6c12.5 12.5 32.8 12.5 45.3 0l160-160c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L224 274.7 86.6 137.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l160 160z" />
                    </svg>
                  )}
                </span>
              </div>
              <div
                className="dropdown-content test"
                style={{ display: isActiveIndustry ? "block" : "none" }}
              >
                {industryData.map((data) => (
                  <div
                    onClick={(e) => {
                      setIsSelectedIndustry(e.target.textContent);
                      setIsActiveIndustry(!isActiveIndustry);
                      globals.industry = e.target.textContent;
                    }}
                    className="item"
                    key={data}
                  >
                    {data}
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>

        <div className="choice-fields">
          <div className="flex-center">
            <img src={bday} alt="" />
          </div>
          <div className="choice-text">My birthday is</div>
          <div className="flex-center">
            <div className="date-input">
              <input
                type="date"
                max=""
                onChange={(event) => {
                  try {
                    const selectedDate = event.target.value;
                    const parts = selectedDate.split("-");
                    const formattedDate = `${parts[0]}-${parts[1]}-${parts[2]}`;
                    const date = new Date(formattedDate);
                    const isoDate = date.toISOString(); // Convert to ISO 8601 format
                    globals.birthday = isoDate;
                    console.log(isoDate);
                  } catch (error) {
                    console.error(error);
                    // Handle the error as needed
                  }
                }}
              />
              <img src={pen} alt="pen" className="pen" />
            </div>
          </div>
        </div>

        <div className="choice-fields">
          <div className="flex-center">
            <img src={clock} alt="" />
          </div>
          <div className="choice-text">In a normal week day, I work for</div>
          <div className="flex-center">
            <div className="dropdown">
              <div
                onClick={(e) => {
                  setIsActiveHours(!isActiveHours);
                }}
                className={
                  isActiveHours
                    ? "dropdown-btn dropdown-true-color"
                    : "dropdown-btn dropdown-false-color"
                }
              >
                {selectedHours}
                <span className="flex-center">
                  {isActiveHours ? (
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      height="1em"
                      viewBox="0 0 320 512"
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
                    >
                      <style
                        dangerouslySetInnerHTML={{
                          __html: "svg{fill:#ffffff}",
                        }}
                      />
                      <path d="M201.4 342.6c12.5 12.5 32.8 12.5 45.3 0l160-160c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L224 274.7 86.6 137.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l160 160z" />
                    </svg>
                  )}
                </span>
              </div>
              <div
                className="dropdown-content test"
                style={{ display: isActiveHours ? "block" : "none" }}
              >
                {hoursData.map((data) => (
                  <div
                    onClick={(e) => {
                      setIsSelectedHours(e.target.textContent);
                      globals.workingHoursInADay = parseInt(
                        e.target.textContent.split("-")[0]
                      );
                      setIsActiveHours(!isActiveHours);
                    }}
                    className="item"
                    key={data}
                  >
                    {data}
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className="btn choice-btn flex-center" onClick={handleNext}>
        Ready to rock! 🔥
      </div>
    </div>
  );
}

export default UserIndustryEtcSelection;

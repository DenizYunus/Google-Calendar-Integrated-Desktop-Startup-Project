import React from "react";
import octobig from "./../utils/octobig.svg";
import sss from "./../utils/sss.svg";
import fff from "./../utils/fff.svg";
import globals from "../components/Globals";

function PleasurePage(props) {
  const handleNext = props.onNext;

  return (
    <div className="content flex-center">
      <div className="meet-text">Pleased to meet you, Adrian! </div>
      <div className="meet-content">
        <div className="picture">
          <img
            src={globals.profilePicture ? globals.profilePicture : octobig}
            alt="octo"
            style={{
              borderRadius: 9999,
              maxWidth: "100%",
              maxHeight: "100%",
              position: "absolute",
              top: "50%",
              left: "50%",
              transform: "translate(-50%, -50%)",
              objectFit: "cover"
            }}
          />
        </div>
        <div>
          <img src={sss} alt="sss" />
        </div>
        <div className="meet-person">
          <div className="meet-photo flex-center">
            <img src={fff} alt="" width="280px" />
          </div>
        </div>
      </div>
      <div className="btn meet-btn flex-center" onClick={handleNext}>
        The pleasure is all mine, OCTO 😊
      </div>
    </div>
  );
}

export default PleasurePage;

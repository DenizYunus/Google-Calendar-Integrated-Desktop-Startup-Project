import React, {useState} from "react";
import fff from "./../utils/fff.svg";
import pen from "./../utils/pen.svg";
import globals from "../components/Globals";

function PlayerOfChoice(props) {
  const handleNext = props.onNext;
const [playerOfChoice, setPlayerOfChoice] = useState("");

  return (
    <div className="content flex-center">
      <div>PROGRESS BAR</div>
      <div className="meet-person">
        <div className="meet-photo flex-center">
          <img
            src={globals.profilePicture ? globals.profilePicture : fff}
            alt=""
            width="280px"
          />
        </div>
      </div>
      <div className="player-text">Adrian, who is your player of choice?</div>
      <div className="name-input" style={{ marginTop: "50px" }}>
        <input
          type="text"
          placeholder="I’m an Entrepreneur..."
          id="name-input"
          onChange={(event) => { setPlayerOfChoice(event.target.value); }}
        />
        <label htmlFor="name-input">
          <img src={pen} alt="pen" className="pen" />
        </label>
      </div>
      <div
        className="btn choice-btn flex-center"
        onClick={() => {
          globals.entrepreneurField = playerOfChoice;
          handleNext();
        }}
      >
        Ready to rock! 🔥
      </div>
    </div>
  );
}

export default PlayerOfChoice;

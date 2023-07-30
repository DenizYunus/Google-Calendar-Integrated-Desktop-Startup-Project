import React, {useState} from "react";
import octomini from "./../utils/octomini.svg";
import globals from "../components/Globals";

function UsernameInputPage(props) {
  const handleNext = props.onNext;
  const [username, setUsername] = useState(""); 

  const handleInputChange = (event) => {
    setUsername(event.target.value); // Update the username state
    globals.Username = event.target.value; // Update the global name variable
  };

  return (
    <div className="content flex-center">
      <div className="head">Choose the way you play with OCTO</div>

      <div className="input-area">
        <div className="input-svg">
          <img src={octomini} alt="octo" />
        </div>
        <div>
          <input type="text" placeholder="Username" value={username} onChange={handleInputChange}/>
        </div>
      </div>
      <div className="input-desc">
        Your username will be linked to your Google account
      </div>

      <div className="btn start-btn flex-center" onClick={ () => {
        if (username.trim() !== "")
        {
            handleNext();
        }
        }}>
        Start! 🚀
      </div>
    </div>
  );
}

export default UsernameInputPage;

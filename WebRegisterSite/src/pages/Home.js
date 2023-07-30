import React, { useState } from "react";
import { useGoogleLogin, hasGrantedAllScopesGoogle } from "@react-oauth/google";
import globals from "./../components/Globals";
import axios from "axios";

import fire from "./../utils/fire.svg";
import person from "./../utils/person.svg";
import cake from "./../utils/cake.svg";
import google from "./../utils/google.svg";
import octo from "./../utils/octo.svg";

function Home(props) {
  const [errorMessage, setErrorMessage] = useState("");
  const handleNext = props.onNext;

  const login = useGoogleLogin({
    onSuccess: (codeResponse) => {
      const hasAccess = hasGrantedAllScopesGoogle(
        codeResponse,
        "https://www.googleapis.com/auth/calendar",
        "https://www.googleapis.com/auth/userinfo.email"
      );

      if (hasAccess) {
        console.log(codeResponse);
        axios
          .post(
            "https://octobackendapiwindows.azurewebsites.net/api/auth/google-signup-start",
            { AuthCode: codeResponse.code }
          )
          .then((response) => {
            // Save the access and refresh tokens to the global state
            globals.accessToken = response.data.accessToken;
            globals.refreshToken = response.data.refreshToken;
            handleNext();
          })
          .catch((error) => {
            if (error.response && error.response.status === 409) {
              setErrorMessage(
                "User already exists. You can go to download sidebar screen directly."
              );
            } else {
              setErrorMessage("Error occurred during sign-up");
            }
          });
      } else
        setErrorMessage(
          "Please provide access to your calendar to take all advantages of OCTO :)"
        );
    },
    flow: "auth-code",
    scope:
      "https://www.googleapis.com/auth/calendar https://www.googleapis.com/auth/userinfo.email",
  });

  return (
    <div className="content flex-center">
      <div className="title">JOIN OCTO TODAY!</div>
      <div className="sub-title">Download the side bar today</div>
      <div className="box flex-center">
        <div className="flex-center">
          <img src={person} alt="person" width="66px" height="71px" />
        </div>
        <div className="box-text">
          <div className="box-text-first">Work is a playground </div>
          <div className="box-text-second">OCTO spices up your work days</div>
        </div>
      </div>

      <div className="box flex-center">
        <div className="flex-center">
          <img src={fire} alt="person" width="66px" height="71px" />
        </div>
        <div className="box-text">
          <div className="box-text-first">Feel empowered</div>
          <div className="box-text-second">Your every step matters to OCTO</div>
        </div>
      </div>

      <div className="box flex-center">
        <div className="flex-center">
          <img src={cake} alt="person" width="66px" height="71px" />
        </div>
        <div className="box-text">
          <div className="box-text-first">Slice your time</div>
          <div className="box-text-second">More fun approach to pomodoro</div>
        </div>
      </div>

      <div className="flex-center">
        <div className="box-text-second">{errorMessage}</div>
      </div>

      {errorMessage ===
        "User already exists. You can go to download sidebar screen directly." && (
        <button
          style={{
            marginTop: "2vh",
            cursor: "pointer",
            width: "10vw",
            height: "5vh",
            background: "rgba(255, 255, 255, 0.2)",
            borderRadius: "10px",
            padding: "10px 15px",
          }}
          onClick={() => {
            props.onGoToDownload();
          }}
        >
          Download Sidebar
        </button>
      )}

      <div className="sign-btn flex-center">
        <div className="flex-center">
          <img src={google} alt="google" />
        </div>
        <div className="sign-btn-text " onClick={() => login()}>
          Sign in with Google
        </div>
      </div>
      {/*<div className="sign-btn flex-center">
        <div className="flex-center">
          <img src={google} alt="google" />
        </div>
        <div className="sign-btn-text " onClick={() => handleNext()}>
          For dev purposes, do not use if you are a commoner.
        </div>
        </div>*/}

      <img className="octo" src={octo} alt="octo" />
    </div>
  );
}

export default Home;

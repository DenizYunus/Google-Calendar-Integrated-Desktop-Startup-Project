import React, { useState } from "react";
import axios from "axios";
import fire from "./../utils/fire.svg";
import camera from "./../utils/camera.svg";
import pen from "./../utils/pen.svg";
import globals from "../components/Globals";

function ProfilePicAndNameInputPage(props) {
  const handleNext = props.onNext;

  const [name, setName] = useState("");
  const [picture, setPicture] = useState(null);

  const handleInputChange = (event) => {
    setName(event.target.value); // Update the username state
    globals.Name = event.target.value; // Update the global name variable
  };

  const handlePictureDrop = (event) => {
    event.preventDefault();
    const file = event.dataTransfer.files[0];
    const reader = new FileReader();
    reader.onload = (event) => {
      setPicture(event.target.result);
    };
    reader.readAsArrayBuffer(file);
  };

  const handlePictureSelect = (event) => {
    const file = event.target.files[0];
    const reader = new FileReader();
    reader.onload = (event) => {
      setPicture(event.target.result);
    };
    reader.readAsArrayBuffer(file);
  };

  const handleReadyClick = async () => {
    try {
      globals.profilePicture = picture;
      // Upload the picture
      if (!picture) {
        console.log("No picture selected");
        return;
      }

      const pictureForm = new FormData();
      pictureForm.append("File", new Blob([picture]), "Avatar.png");

      const uploadResponse = await axios.post(
        "https://octobackendapiwindows.azurewebsites.net/api/user/upload-photo",
        pictureForm,
        {
          headers: {
            "Content-Type": "multipart/form-data",
            Authorization: `Bearer ${globals.accessToken}`,
          },
        }
      );

      // Handle the response of the first API call
      if (uploadResponse.status === 200) {
        console.log("Picture uploaded successfully!");
        console.log(uploadResponse.data.url);

        // Call the second API
        const userData = {
          UserName: globals.Username,
          Name: name,
          ProfilePictureURL: uploadResponse.data.url,
        };

        const signupResponse = await axios.post(
          "https://octobackendapiwindows.azurewebsites.net/api/auth/google-signup-complete",
          userData,
          {
            headers: {
              Authorization: `Bearer ${globals.accessToken}`,
              "Content-Type": "application/json",
            },
          }
        );

        // Handle the response of the second API
        if (signupResponse.status === 200) {
          console.log("Registration completed successfully!");
          handleNext();
        } else {
          console.log("Registration failed: " + signupResponse.data);
        }
      } else {
        console.log("Picture upload failed: " + uploadResponse.data);
      }
    } catch (error) {
      console.log("Error occurred:", error.message);
      if (error.response) {
        console.log("Error response data:", error.response.data);
        console.log("Error response status:", error.response.status);
        console.log("Error response status text:", error.response.statusText);
      }
    }
  };

  const handlePictureClick = () => {
    const input = document.createElement("input");
    input.type = "file";
    input.accept = "image/*";
    input.onchange = (event) => {
      const file = event.target.files[0];
      const reader = new FileReader();
      reader.onload = (event) => {
        setPicture(event.target.result);
      };
      reader.readAsDataURL(file);
    };
    input.click();
  };

  return (
    <div className="content flex-center">
      <div>PROGRESS BAR</div>
      <div
        className="picture"
        onClick={handlePictureClick}
        onDrop={handlePictureDrop}
        onDragOver={(event) => event.preventDefault()}
      >
        {picture ? (
          <img
            src={picture}
            alt="profile"
            style={{
              borderRadius: 9999,
              maxWidth: "100%",
              maxHeight: "100%",
              position: "absolute",
              top: "50%",
              left: "50%",
              transform: "translate(-50%, -50%)",
              objectFit: "cover",
            }}
          />
        ) : (
          <div className="picture-btn flex-center">
            <img src={camera} alt="camera" />
          </div>
        )}
        <input
          type="file"
          accept="image/*"
          onChange={handlePictureSelect}
          style={{ display: "none" }}
        />
      </div>

      <div className="picture-desc">My name is OCTO! And you are... </div>
      <div className="name-input">
        <input
          type="text"
          placeholder="How would you like me to call you?"
          id="name-input"
          value={name}
          onChange={handleInputChange}
        />
        <label htmlFor="name-input">
          <img src={pen} alt="pen" className="pen" />
        </label>
      </div>

      <div
        className="btn flex-center ready-btn"
        onClick={() => {
          console.log(
            globals.Name +
              " " +
              globals.Username +
              " " +
              globals.GoogleIDToken +
              " " +
              globals.accessToken
          );
          handleReadyClick();
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

export default ProfilePicAndNameInputPage;


  /*import React, {useState} from "react";
import fire from "./../utils/fire.svg";
import camera from "./../utils/camera.svg";
import pen from "./../utils/pen.svg";
import globals from "../components/Globals";

function ProfilePicAndNameInputPage(props) {
  const handleNext = props.onNext;

  const [name, setName] = useState(""); 

  const handleInputChange = (event) => {
    setName(event.target.value); // Update the username state
    globals.Name = event.target.value; // Update the global name variable
  };

  return (
    <div className="content flex-center">
      <div>PROGRESS BAR</div>
      <div className="picture">
        <div className="picture-btn flex-center">
          <img src={camera} alt="camera" />
        </div>
      </div>
      {/* <div className="modal">
            <div className="modal-wrapper flex-center">
              <div className="modal-content flex-center">
                <div>
                  <img src={selfie} alt="selfie" />
                </div>
                <div className="text">
                  Drop your image here, or <span>select an avatar </span>
                </div>
                <div></div>
              </div>
            </div>
          </div> *
      <div className="picture-desc">My name is OCTO! And you are... </div>
      <div className="name-input">
        <input
          type="text"
          placeholder="How would you like me to call you?"
          id="name-input"
          value={name} onChange={handleInputChange}
        />
        <label htmlFor="name-input">
          <img src={pen} alt="pen" className="pen" />
        </label>
      </div>

      <div className="btn flex-center ready-btn" onClick={() => {
        console.log(globals.Name + " " + globals.Username + " " + globals.GoogleIDToken + " " + globals.accessToken );
      }}>
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

export default ProfilePicAndNameInputPage;
*/
// }

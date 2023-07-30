import React from "react";
import { useState } from "react";

import img1 from "./../utils/img1.png";
import img2 from "./../utils/img2.png";
import img3 from "./../utils/img3.png";
import img4 from "./../utils/img4.png";
import img5 from "./../utils/img5.png";
import img6 from "./../utils/img6.png";
import octomini from "./../utils/octomini.svg";

function OctoIntroductionPage(props) {
    const handleNext = props.onNext;

  const [toggleState, setToggleState] = useState(1);
  return (
    <div className="content flex-center">
      <div>PROGRESS BAR</div>
      <div className="stranger-title">
        We’re complete strangers! Let’s fix this... I’ll start!
      </div>
      <div className="stranger-area">
        <div className="stranger-octo flex-center">
          <img src={octomini} alt="octo" width="116px" />
        </div>
        <div className="stranger-area-nickname">@octo</div>
        <div className="stranger-area-title">
          empowering the world with new productivity 💜🔥🚀
        </div>
        <div className="tabs">
          <div
            className={toggleState === 1 ? "tab active-tab" : "tab"}
            onClick={() => setToggleState(1)}
          >
            About me
          </div>
          <div
            className={toggleState === 2 ? "tab active-tab" : "tab"}
            onClick={() => setToggleState(2)}
          >
            Photos
          </div>
        </div>
        <div className="tabs-content">
          <div
            className={
              toggleState === 1 ? "tab-content active-content" : "tab-content"
            }
          >
            <div className="tab-title">
              Obsessed with new productivity and passionate to make lives better
              through technology 💙
            </div>
            <div className="tab-subtitle">Player of Choice</div>
            <div className="entrepreneur flex-center content-size">
              Entrepreneur
            </div>
            <div className="tab-subtitle">My World</div>
            <div className="startups flex-center content-size">Startups</div>
            <div className="tab-subtitle">Interests</div>
            <div className="interest-flex">
              <div className="al flex-center content-size">Al</div>
              <div className="travel flex-center content-size">Travel</div>
              <div className="design flex-center content-size">Design</div>
              <div className="art flex-center content-size">Art</div>
            </div>
            <div className="tab-subtitle">Based In</div>
            <div className="berlin flex-center content-size">Berlin📍</div>
            <div className="date">June 18, 2023 🎉</div>
          </div>
          <div
            className={
              toggleState === 2
                ? "tab-content active-content photos"
                : "tab-content photos"
            }
          >
            <div className="img-layout img-container">
              <div className="img-container-one">
                <img className="img1" src={img1} alt="img1" />
                <img className="img2" src={img2} alt="img2" />
                <img className="img3" src={img3} alt="img3" />
              </div>
              <div className="img-container-two">
                <div className="img4">
                  <img src={img4} alt="img4" />
                </div>
                <div className="img5">
                  <img src={img5} alt="img5" />
                </div>
                <div className="img6">
                  <img src={img6} alt="img6" />
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div className="btn stranger-next-btn flex-center" onClick={handleNext}>Next! 🚀</div>
    </div>
  );
}

export default OctoIntroductionPage;

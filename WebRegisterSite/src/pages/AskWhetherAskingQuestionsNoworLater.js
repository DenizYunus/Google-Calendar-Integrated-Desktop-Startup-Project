import React from "react";

function AskWhetherAskingQuestionsNoworLater(props) {
  const handleNext = props.onNext;

  return (
    <div className="content flex-center">
      <div className="shoot-title">I have some crispy questions for you!</div>
      <div className="shoot-subtitle">To see how we can be the best duo </div>
      <div className="btn shoot-btn flex-center" onClick={handleNext}>
        Shoot already! ⚡!
      </div>
      <div className="question">
        I’m in a hurry... <button onClick={props.remindLater} style={{ border: 'none', backgroundColor: 'transparent', color: 'inherit', textDecoration: 'underline', cursor: 'pointer' }}>Remind me later?</button>
      </div>
    </div>
  );
}

export default AskWhetherAskingQuestionsNoworLater;

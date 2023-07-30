import "./App.css";
import { GoogleOAuthProvider } from "@react-oauth/google";

import { useState } from "react";
import Home from "./pages/Home";
import UsernameInputPage from "./pages/UsernameInputPage";
import ProfilePicAndNameInputPage from "./pages/ProfilePicAndNameInputPage";
import PleasurePage from "./pages/PleasurePage";
import AskWhetherAskingQuestionsNoworLater from "./pages/AskWhetherAskingQuestionsNoworLater";
import ServiceSelectPage from "./pages/ServiceSelectPage";
import OctoIntroductionPage from "./pages/OctoIntroductionPage";
import UserIndustryEtcSelection from "./pages/UserIndustryEtcSelection";
import PlayerOfChoice from "./pages/PlayerOfChoice";
import CollaboratePage from "./pages/CollaboratePage";
import DownloadSidebarPage from "./pages/DownloadSidebarPage";

function App() {
  const [view, setView] = useState(1);

  const handleNext = () => {
    setView(view + 1);
  };

  const handlePrev = () => {
    setView(view - 1);
  };

  const renderPage = () => {
    switch (view) {
      case 1:
        return (
          <GoogleOAuthProvider clientId="yourClientIdHere">
            <Home
              onNext={handleNext}
              onGoToDownload={() => {
                setView(11);
              }}
            />{" "}
          </GoogleOAuthProvider>
        );
      case 2:
        return <UsernameInputPage onNext={handleNext} onPrev={handlePrev} />;
      case 3:
        return (
          <ProfilePicAndNameInputPage onNext={handleNext} onPrev={handlePrev} />
        );
      case 4:
        return <PleasurePage onNext={handleNext} onPrev={handlePrev} />;
      case 5:
        return (
          <AskWhetherAskingQuestionsNoworLater
            onNext={handleNext}
            remindLater={() => setView(11)}
            onPrev={handlePrev}
          />
        );
      case 6:
        return <ServiceSelectPage onNext={handleNext} onPrev={handlePrev} />;
      case 7:
        return <OctoIntroductionPage onNext={handleNext} onPrev={handlePrev} />;
      case 8:
        return (
          <UserIndustryEtcSelection onNext={handleNext} onPrev={handlePrev} />
        );
      case 9:
        return <PlayerOfChoice onNext={handleNext} onPrev={handlePrev} />;
      case 10:
        return <CollaboratePage onNext={handleNext} onPrev={handlePrev} />;
      case 11:
        return <DownloadSidebarPage onPrev={handlePrev} />;
      default:
        return null;
    }
  };

  return (
    <div className="bg-color">
      {renderPage()}
      {/*<button
        onClick={() => setView(6)}
        style={{
          position: "absolute",
          width: "100px",
          height: "20px",
          left: "10px",
          top: "20px",
        }}
      ></button>*/}
    </div>
  );
}

export default App;

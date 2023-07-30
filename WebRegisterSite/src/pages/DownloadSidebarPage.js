import React from "react";
import octobig from "./../utils/octobig.svg";

function DownloadSidebarPage() {
  return (
    <div className="content flex-center">
      <div>
        <img src={octobig} alt="octo" className="end-octo" />
      </div>
      <div className="end-title">Thank You! 💜</div>
      <div className="end-subtitle">
        We have everything we need to start achieving your goals together.
      </div>
      <div
        className="btn download-btn flex-center"
        onClick={() => {
          const link = document.createElement("a");
          link.href = "/OCTOSidebar.exe";
          link.setAttribute("download", "");
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);
        }}
      >
        Download Sidebar <span>🎢</span>
      </div>
    </div>
  );
}

export default DownloadSidebarPage;

import React, { useState } from "react";
import EmailForm from "./components/EmailForm";
import VerificationForm from "./components/VerificationForm";
import axios from "axios";

function App() {
  const [email, setEmail] = useState("");
  return (
    <div style={{ padding: "20px" }}>
      <h1>Email авторизация</h1>
      {!email ? (
        <EmailForm onEmailSent={setEmail} />
      ) : (
        <VerificationForm email={email} />
      )}
    </div>
  );
}

export default App;

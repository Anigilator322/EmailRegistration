import React, { useState } from "react";
import { sendVerificationEmail } from "../api/EmailApi";

const EmailForm = ({ onEmailSent }) => {
  const [email, setEmail] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      await sendVerificationEmail(email);
      onEmailSent(email);
    } catch (err) {
      setError("Ошибка при отправке письма. Попробуйте еще раз.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <label>
        Введите ваш Email:
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
      </label>
      <button type="submit" disabled={loading}>
        {loading ? "Отправка..." : "Продолжить"}
      </button>
      {error && <p style={{ color: "red" }}>{error}</p>}
    </form>
  );
};

export default EmailForm;

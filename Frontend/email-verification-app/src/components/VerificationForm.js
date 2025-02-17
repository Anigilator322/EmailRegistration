import React, { useState } from "react";
import { verifyCode } from "../api/EmailApi";

const VerificationForm = ({ email }) => {
  const [code, setCode] = useState("");
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setMessage("");

    try {
      const res = await verifyCode(email, code);
      if(res.isVerified)
      {
        setMessage("Email успешно подтвержден!");
      }
      else{
        setMessage("Неверный код. Попробуйте еще раз.");
      }
    } catch (err) {
      setMessage("Произошла ошибка, попробуйте еще раз.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <label>
        Введите код подтверждения:
        <input
          type="text"
          value={code}
          onChange={(e) => setCode(e.target.value)}
          required
        />
      </label>
      <button type="submit" disabled={loading}>
        {loading ? "Проверка..." : "Подтвердить"}
      </button>
      {message && <p>{message}</p>}
    </form>
  );
};

export default VerificationForm;

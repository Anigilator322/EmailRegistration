import axios from "axios";

const API_BASE_URL = "https://localhost:7049/api/auth";

export const sendVerificationEmail = async (email) => {
  const response = await axios.post(`${API_BASE_URL}/send-code`, { email });
  return response.data;
};

export const verifyCode = async (email, verificationCode) => {
  const response = await axios.post(`${API_BASE_URL}/verify-code`, { email, verificationCode });
  return response.data;
};

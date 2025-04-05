import React, { useState } from "react";
import "./RegisterPage.css";
import AuthForm from "../components/AuthForm/AuthForm";

function RegisterPage() {
  return (
    <div className="registerWrap">
      <AuthForm />
    </div>
  );
}

export default RegisterPage;

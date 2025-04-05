import React from "react";
import "./WelcomePage.css";
import AuthForm from "../components/AuthForm/AuthForm";

function LoginPage() {
  return (
    <div className="loginWrap">
      <AuthForm mode="login"/>
    </div>
  );
}

export default LoginPage;

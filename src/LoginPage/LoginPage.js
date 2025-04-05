import React, { useState } from "react";
import "./LoginPage.css";
import AuthForm from "../components/AuthForm/AuthForm";

function LoginPage() {
  return (
    <div class="loginWrap">
      <AuthForm/>
    </div>
  );
}

export default LoginPage;

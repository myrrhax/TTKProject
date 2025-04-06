import React from "react";
import { Link } from "react-router-dom";
import "./WelcomePage.css";

function WelcomePage() {
  return (
    <main className="welcome-container">
      <div className="welcome-text">
        <h1>Добро пожаловать!</h1>
        <h2>Офисный Веб-Органайзер</h2>
      </div>
      <div className="welcome-divider divider-firstr"></div>
      <div className="welcome-divider divider-second"></div>
      <div className="welcome-button-group">
        <Link to="/login" className="welcome-button">
          Войти
        </Link>
        <Link to="/register" className="welcome-button">
          Зарегистрироваться
        </Link>
      </div>
    </main>
  );
}

export default WelcomePage;

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
      <div className="divider divider-firstr"></div>
      <div className="divider divider-second"></div>
      <div className="button-group">
        <Link to="/login" className="button">
          Войти
        </Link>
        <Link to="/register" className="button">
          Зарегистрироваться
        </Link>
      </div>
    </main>
  );
}

export default WelcomePage;

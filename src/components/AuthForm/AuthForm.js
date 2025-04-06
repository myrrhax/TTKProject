import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./AuthForm.css";

function AuthForm({ mode }) {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    login: "",
    fio: "",
    password: "",
    confirmPassword: "",
  });
  const [errors, setErrors] = useState({});
  const switchMode = (path) => {
    navigate(path);
  };
  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const validate = () => {
    const newErrors = {};

    if (!/^[a-zA-Z]+$/.test(formData.login)) {
      newErrors.login = "Логин должен содержать только латинские буквы";
    }

    if (mode === "register") {
      if (!/^[а-яА-ЯёЁ\s]+$/.test(formData.fio)) {
        newErrors.fio = "ФИО должно содержать только русские буквы";
      }

      if (formData.password !== formData.confirmPassword) {
        newErrors.confirmPassword = "Пароли не совпадают";
      }

      if (
        !/^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d!@#$%^&*()_+}{":;'?/>.<,][^\\]{6,}$/.test(
          formData.password
        )
      ) {
        newErrors.password =
          "Пароль должен содержать латинские буквы, цифры и специальные символы";
      }
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (validate()) {
      let response;
      if (mode == 'login') {
        response = await fetch('http://localhost:5001/api/auth/login', {
          method: "POST",
          body: JSON.stringify({login: formData.login, password: formData.password}),
          headers: {
            "Content-Type": "application/json"
          }
        })} else {
          const [name, surname, secondName] = formData.fio.split(" ")
          console.log("имя: " + name + " фамилия: " + surname + " отчетсво: " + secondName)
          response = await fetch('http://localhost:5001/api/auth/register', {
            method: "POST",
            body: JSON.stringify({login: formData.login, password: formData.password, name: name, surname: surname, secondName: secondName}),
            headers: {
              "Content-Type": "application/json"
            }
          })
      }
      const json = await response.json()
      if (response.status == 200) {
        localStorage.setItem("token", json.token)
        navigate("/tasks");
      }
      else {
        console.log(json); // ToDo Duplicates
      }
    }
  };

  return (
    <form className="auth-form" onSubmit={handleSubmit}>
      <h2>{mode === "login" ? "Вход" : "Регистрация"}</h2>
      <p>Введите свои учетные данные для доступа</p>

      <label>
        Логин
        <input
          type="text"
          name="login"
          value={formData.login}
          onChange={handleChange}
          placeholder="Введите ваш логин"
          required
        />
        {errors.login && <div className="error">{errors.login}</div>}
      </label>

      {mode === "register" && (
        <label>
          ФИО
          <input
            type="text"
            name="fio"
            value={formData.fio}
            onChange={handleChange}
            placeholder="Введите ваше ФИО"
            required
          />
          {errors.fio && <div className="error">{errors.fio}</div>}
        </label>
      )}

      <label>
        Пароль
        <input
          type="password"
          name="password"
          value={formData.password}
          onChange={handleChange}
          placeholder="Введите ваш пароль"
          required
        />
        {errors.password && <div className="error">{errors.password}</div>}
      </label>

      {mode === "register" && (
        <label>
          Повторите пароль
          <input
            type="password"
            name="confirmPassword"
            value={formData.confirmPassword}
            onChange={handleChange}
            placeholder="Введите ваш пароль еще раз"
            required
          />
          {errors.confirmPassword && (
            <div className="error">{errors.confirmPassword}</div>
          )}
        </label>
      )}

      <button type="submit" className="auth-button">
        {mode === "login" ? "Войти" : "Зарегистрироваться"}
      </button>

      {mode === "login" && (
        <p className="switch-mode">
          Нет аккаунта?{" "}
          <span onClick={() => switchMode("/register")}>
            Зарегистрироваться
          </span>
        </p>
      )}
      {mode === "register" && (
        <p className="switch-mode">
          Уже есть аккаунт?{" "}
          <span onClick={() => switchMode("/login")}>Войти</span>
        </p>
      )}
    </form>
  );
}

export default AuthForm;

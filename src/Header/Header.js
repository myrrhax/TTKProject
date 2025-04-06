import React, { useState, useEffect } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import "./Header.css";
import { Newspaper, CalendarCheck, Users } from "lucide-react";

const navItems = [
  { to: "/articles", icon: <Newspaper />, text: "Полезная информация" },
  { to: "/tasks", icon: <CalendarCheck />, text: "Задачи" },
  { to: "/admin", icon: <Users />, text: "Администрирование" },
];

const Header = () => {
  const [user, setUser] = useState(null);
  const token = localStorage.getItem("token");
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUser = async () => {
      if (token) {
        try {
          const decoded = jwtDecode(token);
          const userId =
            decoded[
              "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
            ];

          const response = await fetch(
            `http://localhost:5001/api/users/${userId}`,
            { headers: { Authorization: `Bearer ${token}` } }
          );

          if (!response.ok) {
            if (response.status === 401) {
              localStorage.removeItem("token");
              navigate("/login");
            }
            throw new Error("Ошибка загрузки данных пользователя");
          }

          const data = await response.json();
          setUser(data);
        } catch (error) {
          console.error("Ошибка:", error);
          setUser(null);
        }
      }
    };
    fetchUser();
  }, [token, navigate]);

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/");
  };

  const filteredNavItems = navItems.filter((item) => {
    if (!token) return false;
    if (item.to === "/admin") return user?.role === "admin";
    return true;
  });

  return (
    <header className="header">
      <div className="head">
        <NavLink to="/" style={{ textDecoration: "none" }}>
          <span className="logo">ОВО</span>
        </NavLink>
        <div className="icons-user-wrap">
          <nav className="header-icons">
            {filteredNavItems.map((item) => (
              <NavLink
                key={item.to}
                to={item.to}
                className={({ isActive }) =>
                  `icon-with-text ${isActive ? "active" : ""}`
                }
              >
                {React.cloneElement(item.icon, { className: "icon" })}
                <span className="icon-text">{item.text}</span>
              </NavLink>
            ))}
          </nav>
          {token && (
            <select
              className="user-select"
              onChange={(e) => e.target.value === "logout" && handleLogout()}
            >
              <option>{user?.fullName || "ФИО"}</option>
              <option value="logout">Выйти</option>
            </select>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;

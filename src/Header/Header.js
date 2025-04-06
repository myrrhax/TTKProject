import React, { useState, useEffect } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import "./Header.css";
import { Newspaper, CalendarCheck, Users } from "lucide-react";

const navItems = [
  {
    to: "/articles",
    icon: <Newspaper />,
    text: "Полезная информация",
    requiresAuth: true,
  },
  {
    to: "/tasks",
    icon: <CalendarCheck />,
    text: "Задачи",
    requiresAuth: true,
  },
  {
    to: "/admin",
    icon: <Users />,
    text: "Администрирование",
    requiresAuth: true,
    requiredRole: "admin",
  },
];

const Header = () => {
  const [hasToken, setHasToken] = useState(false);
  const [userRole, setUserRole] = useState(null);
  const [fullName, setFullName] = useState("ФИО");
  const navigate = useNavigate();

  useEffect(() => {
    const checkAuth = async () => {
      const token = localStorage.getItem("token");
      if (token) {
        setHasToken(true);
        const decoded = parseJwt(token);
        setUserRole(decoded.role);
        if (decoded.id) {
          try {
            const response = await fetch(
              `http://localhost:5001/api/users/${decoded.id}`
            );
            const data = await response.json();
            setFullName(data.fullName || "ФИО");
          } catch (error) {
            console.error("Error fetching user data:", error);
            setFullName("ФИО");
          }
        }
      } else {
        setHasToken(false);
        setUserRole(null);
      }
    };
    checkAuth();
  }, [navigate]);

  const parseJwt = (token) => {
    try {
      const base64Url = token.split(".")[1];
      const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
      return JSON.parse(atob(base64));
    } catch (e) {
      return {};
    }
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    setHasToken(false);
    setUserRole(null);
    navigate("/");
  };

  return (
    <header className="header">
      <div className="head">
        <NavLink to={"/"} style={{ textDecoration: "none" }}>
          <span className="logo">ОВО</span>
        </NavLink>
        <div className="icons-user-wrap">
          {hasToken && (
            <>
              <nav className="header-icons">
                {navItems
                  .filter(
                    (item) =>
                      !item.requiresAuth ||
                      (hasToken &&
                        (!item.requiredRole || item.requiredRole === userRole))
                  )
                  .map((item) => (
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
              <select
                className="user-select"
                onChange={(e) => e.target.value === "logout" && handleLogout()}
              >
                <option>{fullName}</option>
                <option value="logout">Выйти</option>
              </select>
            </>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;

import React, { useState, useEffect } from "react";
import { jwtDecode } from "jwt-decode";
import "./Header.css";
import { Newspaper, CalendarCheck, Users } from "lucide-react";
import { NavLink, useNavigate } from "react-router-dom";

const Header = () => {
  const navigate = useNavigate();
  const [userRole, setUserRole] = useState(null);

  useEffect(() => {
    const checkToken = () => {
      const token = localStorage.getItem("token");
      if (token) {
        try {
          const decoded = jwtDecode(token);
          setUserRole(decoded.role);
        } catch (error) {
          console.error("Invalid token");
          setUserRole(null);
        }
      } else {
        setUserRole(null);
      }
    };
    checkToken();
  }, []);

  const handleLogout = (e) => {
    if (e.target.value === "Выйти") {
      localStorage.removeItem("token");
      setUserRole(null);
      navigate("/");
    }
  };

  const commonNavItems = [
    { to: "/articles", icon: <Newspaper />, text: "Полезная информация" },
    { to: "/tasks", icon: <CalendarCheck />, text: "Задачи" },
  ];

  const adminNavItem = {
    to: "/admin",
    icon: <Users />,
    text: "Администрирование",
  };

  const navItems =
    userRole === "admin" ? [...commonNavItems, adminNavItem] : commonNavItems;

  return (
    <header className="header">
      <div className="head">
        <NavLink to={"/"} style={{ textDecoration: "none" }}>
          <span className="logo">ОВО</span>
        </NavLink>
        <div className="icons-user-wrap">
          <nav className="header-icons">
            {navItems.map((item) => (
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
          <select className="user-select" onChange={handleLogout} value={""}>
            <option>ФИО</option>
            <option>Выйти</option>
          </select>
        </div>
      </div>
    </header>
  );
};

export default Header;

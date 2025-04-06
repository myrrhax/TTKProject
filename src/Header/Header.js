import React from "react";
import "./Header.css";
import { Newspaper, CalendarCheck, Users } from "lucide-react";
import { NavLink, useNavigate } from "react-router-dom"; // [[8]]

const navItems = [
  {
    to: "/articles",
    icon: <Newspaper />,
    text: "Полезная информация",
  },
  {
    to: "/tasks",
    icon: <CalendarCheck />,
    text: "Задачи",
  },
  {
    to: "/admin",
    icon: <Users />,
    text: "Администрирование",
  },
];

const Header = () => {
  const navigate = useNavigate();

  const handleLogout = (e) => {
    if (e.target.value === "Выйти") {
      localStorage.removeItem("token");
      navigate("/");
    }
  };

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

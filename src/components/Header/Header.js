import React from "react";
import "./Header.css";
import { Newspaper, CalendarCheck, Users } from "lucide-react";
import { NavLink } from "react-router-dom";

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
  return (
    <header className="header">
      <div className="head">
        <span className="logo">ОВО</span>
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
          <div className="user-dropdown">
            <img
              src="https://randomuser.me/api/portraits/men/32.jpg"
              alt="User avatar"
              className="avatar"
            />
            <select className="user-select">
              <option>Derek Alvarado</option>
              <option>Profile</option>
              <option>Logout</option>
            </select>
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;

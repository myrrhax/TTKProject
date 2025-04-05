import React from "react";
import "./Header.css";
import { Bell, MessageCircle, Users } from "lucide-react";

const Header = () => {
  return (
    <header className="header">
      <div className="head">
        <div className="header-icons">
          <Bell className="icon" />
          <MessageCircle className="icon" />
          <Users className="icon" />
        </div>
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
    </header>
  );
};

export default Header;

import React from "react";
import "./TaskCard.css";
import { Pencil } from "lucide-react";

const TaskCard = () => {
  return (
    <div className="task-card">
      <div className="task-card-header">
        <div>
          <div>
            <h3 className="task-title">Set Up Backend Infrastructure</h3>
          </div>
          <div className="task-dates">
            <span className="dead-date">
              <div className="status">
                <span>06.08.25</span>
                <span className="arrow-right"></span>
                <span className="task-deadline">15.08.25</span>
              </div>
            </span>
            <span>
              <span className="status-dot"></span>
              <span className="task-priority">Высокий</span>
            </span>
          </div>
          <p className="task-description">
            Prepare the server environment and database for app development.
          </p>
        </div>
      </div>

      <div className="content-card">
        <div className="action-side">
          <div className="task-user-priority">
            <div className="task-user">
              <img
                className="task-avatar"
                src="https://ui.shadcn.com/avatars/01.png"
                alt="Derek"
              />
              <select className="task-user-select">
                <option>Derek Alvarado</option>
              </select>
            </div>
          </div>
          <div className="task-actions">
            <button className="change-button">
              <Pencil className="task-edit-icon" />
            </button>
            <button className="task-button">Выполнить</button>
            <button className="task-status">В работу</button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default TaskCard;

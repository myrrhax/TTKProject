import React from "react";
import "./TaskCard.css";
import { Pencil } from "lucide-react";

const TaskCard = () => {
  return (
    <div className="task-card">
      <div className="task-card-header">
        <div>
          <h3 className="task-title">Set Up Backend Infrastructure</h3>
          <div className="task-dates">
            <span className="dead-date">
              <span>06.08.25</span>
              <span className="task-deadline">15.08.25</span>
            </span>
            <span className="task-priority">Высокий</span>
          </div>
          <p className="task-description">
            Prepare the server environment and database for app development.
          </p>
        </div>
        <Pencil className="task-edit-icon" />
      </div>

      <div class="content-card">
        <div class="action-side">
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
            <button className="task-button">Выполнить</button>
            <button className="task-status">В работу</button>
          </div>
        </div>
        <div class="info_side"></div>
      </div>
    </div>
  );
};

export default TaskCard;

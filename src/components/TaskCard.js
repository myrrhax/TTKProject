import React from "react";
import "./TaskCard.css";
import { Clock } from "lucide-react";

const TaskCard = () => {
  return (
    <div className="task-card">
      <div className="task-card-header">
        <div>
          <h3 className="task-title">Set Up Backend Infrastructure</h3>
          <p className="task-description">
            Prepare the server environment and database for app development.
          </p>
        </div>
        <Clock className="task-clock" />
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
            <span className="task-status">В работу</span>
          </div>
        </div>
        <div class="info_side">
          <div className="task-priority">Высокий</div>
          <div className="task-dates">
            <span>06.08.25</span>
            <span className="task-deadline">15.08.25</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default TaskCard;

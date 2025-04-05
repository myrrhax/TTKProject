import React, { useState } from "react";
import "./TaskPage.css";
import TaskCard from "../components/TaskCard/TaskCard";

function TaskPage() {
  return (
    <div class="task-component">
      <div className="task-folder">
        <h2>Отложенные задачи</h2>
        <TaskCard />
      </div>
      <div className="task-folder">
        <h2>Задачи в работе</h2>
        <TaskCard />
      </div>
      <div className="task-folder">
        <h2>Выполненные задачи</h2>
        <TaskCard />
      </div>
    </div>
  );
}

export default TaskPage;

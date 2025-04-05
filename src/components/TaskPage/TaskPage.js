import React, { useState } from "react";
import "./TaskPage.css";
import TaskCard from "../TaskCard/TaskCard";

function TaskPage() {
  return (
    <div class="task-component">
      <div className="task-folder">
        <h2>Отложенные задачи</h2>
        <TaskCard />
      </div>
      <div className="task-folder">Задачи в работе</div>
      <div className="task-folder">Выполеннные задачи</div>
    </div>
  );
}

export default TaskPage;

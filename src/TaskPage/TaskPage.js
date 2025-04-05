import React, { useState } from "react";
import "./TaskPage.css";
import TaskModal from "../components/Modal/Modal";
import TaskCard from "../components/TaskCard/TaskCard";

function TaskPage() {
  const [modalOpen, setModalOpen] = useState(false);
  return (
    <div className="taskWrap">
      <TaskModal isOpen={modalOpen} onClose={() => setModalOpen(false)} />
      <div class="title-bar">
        <h1>Задачи</h1>
        <button onClick={() => setModalOpen(true)}>Создать задачу</button>
      </div>
      <div class="task-component">
        <div className="task-folder">
          <h2>Отложенные задачи</h2>
          <TaskCard />
        </div>
        <div className="task-folder">
          <h2>Задачи в работе</h2>
        </div>
        <div className="task-folder">
          <h2>Выполеннные задачи</h2>
        </div>
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

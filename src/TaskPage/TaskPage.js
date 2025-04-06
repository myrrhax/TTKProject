import React, { useState } from "react";
import "./TaskPage.css";
import TaskModal from "../components/Modal/Modal";
import TaskCard from "../components/TaskCard/TaskCard";
import TaskCardInWork from "../components/TaskCard/TaskCardInWork";
import TaskCardDone from "../components/TaskCard/TaskCardDone";
import { Clock } from "lucide-react";

function TaskPage() {
  const [modalOpen, setModalOpen] = useState(false);
  return (
    <div className="taskWrap">
      <TaskModal isOpen={modalOpen} onClose={() => setModalOpen(false)} />
      <div className="title-bar">
        <h1>Задачи</h1>
        <div class="button-right">
          <button onClick={() => setModalOpen(true)}>Создать задачу</button>
          <a href="/log_tasks">
            <button class="button-block" onClick={() => setModalOpen(true)}>
              <Clock className="task-edit-icon" />
            </button>
          </a>
        </div>
      </div>
      <div className="task-component">
        <div className="task-folder">
          <h2>Отложенные задачи</h2>
          <TaskCard />
          <TaskCard />
        </div>
        <div className="task-folder">
          <h2>Задачи в работе</h2>
          <TaskCardInWork />
        </div>
        <div className="task-folder">
          <h2>Выполеннные задачи</h2>
          <TaskCardDone />
        </div>
      </div>
    </div>
  );
}

export default TaskPage;

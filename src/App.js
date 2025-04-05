import React, { useState } from "react";
import TaskModal from "./components/Modal/Modal"; // Убедись в правильном пути!
import TaskPage from "./components/TaskPage/TaskPage";
import Header from "./components/Header/Header";
import "./App.css";

function App() {
  const [modalOpen, setModalOpen] = useState(false);

  return (
    <div className="App">
      <Header />
      <div class="title-bar">
        <h1>Задачи</h1>
        <button onClick={() => setModalOpen(true)}>Создать задачу</button>
      </div>
      <TaskModal isOpen={modalOpen} onClose={() => setModalOpen(false)} />
      <TaskPage />
    </div>
  );
}

export default App;

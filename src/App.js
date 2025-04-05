import React, { useState } from "react";
import TaskModal from "./components/Modal/Modal"; // Убедись в правильном пути!
import TaskPage from "./TaskPage/TaskPage";
import Header from "./components/Header/Header";
import AdminPage from "./AdminPage/AdminPage";
import ReaderPage from "./ReaderPage/Reader";
import ArticleCard from "./components/Articles/ArticleCard";
import "./App.css";

function App() {
  const [modalOpen, setModalOpen] = useState(false);

  return (
    <div className="App">
      <Header />
      <TaskModal isOpen={modalOpen} onClose={() => setModalOpen(false)} />
      {/* <TaskPage /> */}
      <ArticleCard
        title="Название"
        date="01.01.2025"
        author="Имя Фамилия Отчество"
        imageUrl="images/wb_5949216_5.jpg"
      />
    </div>
  );
}

export default App;

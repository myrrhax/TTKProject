import React, { useState } from "react";
import "./ArticlesPage.css";
import ArticleCard from "../components/Articles/ArticleCard";
import TaskModal from "../components/Modal/Modal";
import { Clock } from "lucide-react";

function ArticlesPage() {
  const [modalOpen, setModalOpen] = useState(false);
  return (
    <div className="article-page">
      <div className="title-bar">
        <h1>Полезная информация</h1>
        <div class="button-right">
          <button onClick={() => setModalOpen(true)}>Создать статью</button>
          <a href="/log_tasks">
            <button class="button-block" onClick={() => setModalOpen(true)}>
              <Clock className="task-edit-icon" />
            </button>
          </a>
        </div>
      </div>
      <div className="article-section">
        <ArticleCard
          title="Название"
          date="01.01.2025"
          author="Имя Фамилия Отчество"
          imageUrl="images/wb_5949216_1.jpg"
        />
        <ArticleCard
          title="Название"
          date="01.01.2025"
          author="Имя Фамилия Отчество"
          imageUrl="images/wb_5949216_2.jpg"
        />
        <ArticleCard
          title="Название"
          date="01.01.2025"
          author="Имя Фамилия Отчество"
          imageUrl="images/wb_5949216_3.jpg"
        />
        <ArticleCard
          title="Название"
          date="01.01.2025"
          author="Имя Фамилия Отчество"
          imageUrl="images/wb_5949216_4.jpg"
        />
        <ArticleCard
          title="Название"
          date="01.01.2025"
          author="Имя Фамилия Отчество"
          imageUrl="images/wb_5949216_5.jpg"
        />
      </div>
    </div>
  );
}

export default ArticlesPage;

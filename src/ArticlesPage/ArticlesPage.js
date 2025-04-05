import React from "react";
import "./ArticlesPage.css";
import ArticleCard from "../components/Articles/ArticleCard";

function ArticlesPage() {
  return (
    <div className="article-page">
      <h2>Новые статьи</h2>
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

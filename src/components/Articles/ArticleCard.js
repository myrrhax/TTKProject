import React from "react";
import "./ArticleCard.css";

const ArticleCard = ({ title, date, author, imageUrl }) => {
  return (
    <div className="article-card">
      <div
        className="article-card__image"
        style={{ backgroundImage: `url(${imageUrl})` }}
      >
        <div className="article-card__logo">GЭП</div>
      </div>
      <div className="article-card__content">
        <h2 className="article-card__title">{title}</h2>
        <p className="article-card__date">{date}</p>
        <p className="article-card__author">Автор: {author}</p>
      </div>
    </div>
  );
};

export default ArticleCard;

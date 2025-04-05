import React from "react";
import "./Reader.css";

const Reader = () => {
  return (
    <div className="page-container">
      <aside className="sidebar">
        <div className="recent-picks">
          <h3>Недавние подборки</h3>
          <div className="recent-item">
            <img
              src="https://via.placeholder.com/300x200"
              alt="UX review presentations"
            />
            <div className="caption">
              <h4>UX review presentations</h4>
              <p>
                How to make your design presentations hit the mark with your
                colleagues and impress your managers
              </p>
            </div>
          </div>
        </div>
      </aside>
      <main className="article-content">
        <div className="meta">
          <span className="date">Sunday, 1 Jan 2023</span>
          <span className="tags"># для неё #14 февраля</span>
        </div>
        <h1>Топ 10 подарков для нее на 14 февраля в 2025 году</h1>
        <img
          className="header-image"
          src="https://via.placeholder.com/800x400"
          alt="article header"
        />
        <p className="lead">
          «14 февраля» — день, когда хочется выражать свои чувства ярко и
          незабываемо. Но как выбрать идеальный подарок? Для своей возлюбленной,
          которая оценит, растает и останется в памяти? Мы подготовили для вас
          топ-10 уникальных идей подарков на 2024 год — от романтичных сюрпризов
          до практичных вариантов, которые подойдут для неё, для неё и даже для
          тех, кто только начинает отношения.
        </p>

        <h2>Брюки в корейском стиле</h2>
        <img src="https://via.placeholder.com/700x400" alt="pants example" />
      </main>
    </div>
  );
};

export default Reader;

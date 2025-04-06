import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./components/Header/Header";

import LoginPage from "./LoginPage/LoginPage";
import RegisterPage from "./RegisterPage/RegisterPage";
import TaskPage from "./TaskPage/TaskPage";
import AdminPage from "./AdminPage/AdminPage";
import ReaderPage from "./ReaderPage/Reader";
import ArticlesPage from "./ArticlesPage/ArticlesPage";
import WriterPage from "./WriterPage/WriterPage";
import "./App.css";

function App() {
  return (
    <Router>
      <div className="App">
        <Header />
        <Routes>
          <Route path="/" element={<WelcomePage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/tasks" element={<TaskPage />} />
          <Route path="/admin" element={<AdminPage />} />
          <Route path="/articles" element={<ArticlesPage />} />
          <Route path="/read" element={<ReaderPage />} />
          <Route path="/write" element={<WriterPage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;

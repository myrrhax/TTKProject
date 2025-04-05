import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Header from "./components/Header/Header";
import LoginPage from "./LoginPage/LoginPage";
import RegisterPage from "./RegisterPage/RegisterPage";
import TaskPage from "./TaskPage/TaskPage";
import AdminPage from "./AdminPage/AdminPage";
import ReaderPage from "./ReaderPage/Reader";
import "./App.css";

function App() {
  return (
    <Router>
      <div className="App">
        <Header />
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/tasks" element={<TaskPage />} />
          <Route path="/admin" element={<AdminPage />} />
          <Route path="/articles" element={<ReaderPage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;

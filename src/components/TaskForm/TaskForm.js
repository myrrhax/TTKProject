import React, { useState } from "react";
import "./TaskForm.css";

function TaskForm({ onClose }) {
  const [title, setTitle] = useState("");
  const [assignee, setAssignee] = useState("");
  const [priority, setPriority] = useState("low");
  const [description, setDescription] = useState("");
  const [deadline, setDeadline] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    const taskData = { title, assignee, priority, description, deadline };
    console.log("📌 Submitted Task:", taskData);
    onClose();
  };

  const handleReset = () => {
    setTitle("");
    setAssignee("");
    setPriority("low");
    setDescription("");
    setDeadline("");
  };

  return (
    <form className="task-form" onSubmit={handleSubmit}>
      <h2>Создание задачи</h2>
      {/* поля формы */}
      <label htmlFor="title">Название задачи</label>
      <input
        id="title"
        type="text"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        placeholder="Введите название"
        required
      />

      <label htmlFor="assignee">Ответственный</label>
      <select
        id="assignee"
        value={assignee}
        onChange={(e) => setAssignee(e.target.value)}
        required
      >
        <option value="" disabled>
          Выберите исполнителя
        </option>
        <option value="Олег">ОЛЕГ</option>
        <option value="Иван">Иван</option>
        <option value="Василий">Василий</option>
        <option value="Степан">Степан</option>
        <option value="Илья">Илья</option>
      </select>

      <label htmlFor="description">Описание</label>
      <textarea
        id="description"
        rows="5"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        placeholder="Опишите задачу"
        required
      ></textarea>

      <label htmlFor="deadline">Дедлайн</label>
      <input
        id="deadline"
        type="date"
        value={deadline}
        onChange={(e) => setDeadline(e.target.value)}
        required
      />

      <label>Приоритет</label>
      <div className="priority-options">
        <label>
          <input
            type="radio"
            value="high"
            checked={priority === "high"}
            onChange={(e) => setPriority(e.target.value)}
          />
          Высокий
        </label>
        <label>
          <input
            type="radio"
            value="medium"
            checked={priority === "medium"}
            onChange={(e) => setPriority(e.target.value)}
          />
          Средний
        </label>
        <label>
          <input
            type="radio"
            value="low"
            checked={priority === "low"}
            onChange={(e) => setPriority(e.target.value)}
          />
          Низкий
        </label>
      </div>

      <div className="form-buttons">
        <button type="submit">Создать</button>
        <button type="button" onClick={handleReset}>
          Очистить
        </button>
      </div>
    </form>
  );
}

export default TaskForm;

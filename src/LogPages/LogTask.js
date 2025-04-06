import React, { useState } from "react";
import "./Log.css";
import { Pencil, Trash, Key } from "lucide-react";

const usersData = [
  {
    id: "1fa85f64-5717-4562-b3fc-2c963f66afa1",
    title: "Связать бэк и фронт",
    description:
      "Проанализировать эндпоинты и вывести предложения по проработке",
    dueDate: "2025-04-06T03:01:03.826Z",
    priority: "High",
    status: "Done",
    assignedUserId: "3fa85f64-1234-4562-b3fc-2c963f66afa6",
  },
  {
    id: "1fa85f64-5717-4562-b3fc-2c963f66afa2",
    title: "Реализовать аутентификацию",
    description: "Настроить AuthService и интегрировать JWT",
    dueDate: "2025-04-08T12:00:00.000Z",
    priority: "High",
    status: "In Progress",
    assignedUserId: "3fa85f64-1234-4562-b3fc-2c963f66afa7",
  },
  {
    id: "1fa85f64-5717-4562-b3fc-2c963f66afa3",
    title: "Создать микросервис задач",
    description: "Создать TasksService с CRUD, валидацией и логированием",
    dueDate: "2025-04-10T09:30:00.000Z",
    priority: "Medium",
    status: "To Do",
    assignedUserId: "3fa85f64-1234-4562-b3fc-2c963f66afa8",
  },
  {
    id: "1fa85f64-5717-4562-b3fc-2c963f66afa4",
    title: "Подключить Swagger",
    description: "Сгенерировать документацию для всех сервисов",
    dueDate: "2025-04-07T15:45:00.000Z",
    priority: "Low",
    status: "Done",
    assignedUserId: "3fa85f64-1234-4562-b3fc-2c963f66afa6",
  },
  {
    id: "1fa85f64-5717-4562-b3fc-2c963f66afa5",
    title: "Оптимизировать базу данных",
    description: "Добавить индексы, проверить нормализацию, почистить миграции",
    dueDate: "2025-04-09T18:15:00.000Z",
    priority: "Medium",
    status: "In Review",
    assignedUserId: "3fa85f64-1234-4562-b3fc-2c963f66afa9",
  },
  // остальные задачи...
];

const StatusDot = ({ status }) => (
  <span className={`status-dot ${status.toLowerCase().replace(" ", "-")}`} />
);

export default function UserTable() {
  const [sortConfig, setSortConfig] = useState({ key: "", direction: "asc" });

  const sortedUsers = [...usersData].sort((a, b) => {
    const { key, direction } = sortConfig;
    if (!key) return 0;

    let valA = a[key];
    let valB = b[key];

    if (key === "dueDate") {
      valA = new Date(valA);
      valB = new Date(valB);
    } else {
      valA = valA.toString().toLowerCase();
      valB = valB.toString().toLowerCase();
    }

    if (valA < valB) return direction === "asc" ? -1 : 1;
    if (valA > valB) return direction === "asc" ? 1 : -1;
    return 0;
  });

  const handleSort = (key) => {
    setSortConfig((prev) => {
      if (prev.key === key) {
        return { key, direction: prev.direction === "asc" ? "desc" : "asc" };
      }
      return { key, direction: "asc" };
    });
  };

  const getSortSymbol = (key) => {
    if (sortConfig.key !== key) return "";
    return sortConfig.direction === "asc" ? " ↑" : " ↓";
  };

  return (
    <div className="table-container">
      <table className="user-table">
        <thead>
          <tr>
            <th onClick={() => handleSort("title")} className="sortable">
              Задача{getSortSymbol("title")}
            </th>
            <th onClick={() => handleSort("description")} className="sortable">
              Описание{getSortSymbol("description")}
            </th>
            <th onClick={() => handleSort("dueDate")} className="sortable">
              Последнее изменение{getSortSymbol("dueDate")}
            </th>
            <th onClick={() => handleSort("priority")} className="sortable">
              Приоритет{getSortSymbol("priority")}
            </th>
            <th onClick={() => handleSort("status")} className="sortable">
              Статус{getSortSymbol("status")}
            </th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {sortedUsers.map((user) => (
            <tr key={user.id}>
              <td>{user.title}</td>
              <td>{user.description}</td>
              <td>{new Date(user.dueDate).toLocaleDateString()}</td>
              <td>{user.priority}</td>
              <td>
                <div className="status-cell">
                  <StatusDot status={user.status} />
                  {user.status}
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

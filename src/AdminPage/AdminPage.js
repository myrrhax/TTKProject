import React, { useState } from "react";
import "./AdminPage.css";
import { Pencil } from "lucide-react";
import { Trash } from "lucide-react";
import { Key } from "lucide-react";

const usersData = [
  {
    id: 1,
    name: "Alyvia Kelley",
    role: "Администратор",
    status: "green",
    date: "06/18/1978",
    locked: true,
  },
  {
    id: 2,
    name: "Jaiden Nixon",
    role: "Пользователь",
    status: "green",
    date: "09/30/1983",
  },
  {
    id: 3,
    name: "Ace Foley",
    role: "Пользователь",
    status: "black",
    date: "12/03/1985",
  },
  {
    id: 4,
    name: "Nickoli Schmidt",
    role: "Rejected",
    status: "red",
    date: "03/22/1956",
  },
  {
    id: 5,
    name: "Clayton Charles",
    role: "Approved",
    status: "green",
    date: "10/14/1971",
  },
  // другие...
];

const RoleDot = ({ color }) => <span className={`role-dot ${color}`} />;

export default function UserTable() {
  const [sortConfig, setSortConfig] = useState({ key: "", direction: "asc" });

  const sortedUsers = [...usersData].sort((a, b) => {
    const { key, direction } = sortConfig;

    if (!key) return 0;

    let valA = a[key];
    let valB = b[key];

    if (key === "date") {
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
            <th>Логин</th>
            <th onClick={() => handleSort("name")} className="sortable">
              ФИО{getSortSymbol("name")}
            </th>
            <th onClick={() => handleSort("role")} className="sortable">
              Роль{getSortSymbol("role")}
            </th>
            <th onClick={() => handleSort("date")} className="sortable">
              Дата регистрации{getSortSymbol("date")}
            </th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {sortedUsers.map((user, index) => (
            <tr key={user.id}>
              <td>{index + 1}</td>
              <td>{user.name}</td>
              <td>
                <div className="role-cell">
                  <RoleDot color={user.status} />
                  {user.role}
                </div>
              </td>
              <td>{user.date}</td>
              <td className="actions-cell">
                <button title="Details">
                  <Key className="task-edit-icon" />
                </button>
                <button title="Edit">
                  <Pencil className="task-edit-icon" />
                </button>
                <button title="Delete">
                  <Trash className="task-edit-icon" />
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

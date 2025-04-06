import React, { useState, useEffect } from "react";
import "./AdminPage.css";
import { Pencil } from "lucide-react";
import { Trash } from "lucide-react";
import { Key } from "lucide-react";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";
import protectedRequestFabric from "../requestFabric";

const RoleDot = ({ color }) => <span className={`role-dot ${color}`} />;

export default function UserTable() {
  const navigate = useNavigate();
  const [users, setUsers] = useState([])
  
  useEffect(() => {
    const fetchData = async () => {
      const token = localStorage.getItem('token');
      const roleSchema = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
  
      if (!token) {
        navigate('/');
        return;
      }
  
      try {
        const decoded = jwtDecode(token);
        // if (decoded[roleSchema] === 'user') {
        //   navigate('/');
        // }
      } catch (error) {
        console.error('Ошибка при декодировании токена:', error);
        navigate('/');
        return;
      }
  
      try {
        const response = await protectedRequestFabric("http://localhost:5001/api/users", "GET");
        if (response.ok) {
          const json = await response.json();
          console.log(json)
          setUsers(json.users)
        }
      } catch (error) {
        console.error("Ошибка при загрузке пользователей:", error);
      }
    };
  
    fetchData();
  }, [navigate]);

  const [sortConfig, setSortConfig] = useState({ key: "", direction: "asc" });

  const sortedUsers = [...users].sort((a, b) => {
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

  const deleteUser = async (userId) => {
    const response = await protectedRequestFabric("http://localhost:5001/api/users/" + userId, 'DELETE')
    if (response.ok) {
      users.find(user => user.userId == userId).status = false;
    }
  }

  return (
    <div className="table-container">
      <table className="user-table">
        <thead>
          <tr>
            <th>Id</th>
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
            <th>Статус</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {sortedUsers.map((user, index) => (
            <tr key={user.userId}>
              <td>{index + 1}</td>
              <td>{user.login}</td>
              <td>{user.fullName}</td>
              <td>
                <div className="role-cell">
                  <RoleDot color={user.status} />
                  {user.role}
                </div>
              </td>
              <td>{user.creationDate}</td>
              <td>{user.isDeleted ? "Удален" : "Существует"}</td>
              <td className="actions-cell">
                <button title="Details">
                  <Key className="task-edit-icon" />
                </button>
                <button title="Edit">
                  <Pencil className="task-edit-icon" />
                </button>
                <button title="Delete">
                <Trash className="task-edit-icon" onClick={() => deleteUser(user.userId)} />
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

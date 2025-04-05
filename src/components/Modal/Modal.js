import React from "react";
import "./Modal.css";
import TaskForm from "../TaskForm/TaskForm";

function Modal({ isOpen, onClose }) {
  if (!isOpen) return null;

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-button" onClick={onClose}>
          ×
        </button>
        <TaskForm onClose={onClose} />
      </div>
    </div>
  );
}

export default Modal;

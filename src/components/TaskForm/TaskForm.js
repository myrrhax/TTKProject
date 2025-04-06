import React, { useEffect, useState } from "react";

import { LexicalComposer } from "@lexical/react/LexicalComposer";
import { RichTextPlugin } from "@lexical/react/LexicalRichTextPlugin";
import { ContentEditable } from "@lexical/react/LexicalContentEditable";
import { HistoryPlugin } from "@lexical/react/LexicalHistoryPlugin";
import { OnChangePlugin } from "@lexical/react/LexicalOnChangePlugin";
import { HeadingNode, QuoteNode } from "@lexical/rich-text";
import { ListNode, ListItemNode } from "@lexical/list";
import { CodeNode } from "@lexical/code";
import { LinkNode, AutoLinkNode } from "@lexical/link";
import { MarkdownShortcutPlugin } from "@lexical/react/LexicalMarkdownShortcutPlugin";
import { LexicalErrorBoundary } from "@lexical/react/LexicalErrorBoundary";
import { TRANSFORMERS, $convertToMarkdownString } from "@lexical/markdown";
import { FloatingToolbar } from "./FloatingToolbar";
import { useLexicalComposerContext } from "@lexical/react/LexicalComposerContext";
import { $getRoot } from "lexical";
import "./TaskForm.css";

const transformers = Object.values(TRANSFORMERS).flat();

function FocusOnMount() {
  const [editor] = useLexicalComposerContext();

  useEffect(() => {
    editor.focus();
  }, [editor]);

  return null;
}

const editorConfig = {
  namespace: "MyEditor",
  theme: {},
  onError(error) {
    throw error;
  },
  nodes: [
    HeadingNode,
    ListNode,
    ListItemNode,
    QuoteNode,
    CodeNode,
    AutoLinkNode,
    LinkNode,
  ],
};
function SaveButtonMarkdown() {
  const [editor] = useLexicalComposerContext();

  const handleSave = () => {
    editor.getEditorState().read(() => {
      const markdown = $convertToMarkdownString(transformers);
      console.log("📄 Markdown:\n", markdown);
      alert("Markdown выведен в консоль ✅");
    });
  };

  return (
    <button
      onClick={handleSave}
      style={{
        marginTop: "1rem",
        padding: "0.5rem 1rem",
        backgroundColor: "#28a745",
        color: "white",
        border: "none",
        borderRadius: "4px",
        cursor: "pointer",
      }}
    >
      Сохранить как Markdown
    </button>
  );
}
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
      {/* <textarea
        id="description"
        rows="5"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        placeholder="Опишите задачу"
        required
      ></textarea> */}
      <LexicalComposer initialConfig={editorConfig}>
        <FocusOnMount />
        <div
          style={{
            border: "1px solid #ccc",
            borderRadius: "6px",
            padding: "1rem",
            minHeight: "200px",
            backgroundColor: "white",
            position: "relative",
          }}
        >
          <RichTextPlugin
            contentEditable={
              <ContentEditable
                style={{
                  outline: "none",
                  minHeight: "150px",
                  fontSize: "16px",
                  position: "relative",
                  zIndex: 1,
                }}
              />
            }
            placeholder={
              <div
                style={{
                  color: "#aaa",
                  position: "absolute",
                  top: "1rem",
                  left: "1rem",
                  pointerEvents: "none",
                  zIndex: 0,
                }}
              >
                Введите текст...
              </div>
            }
            ErrorBoundary={LexicalErrorBoundary}
          />
          <HistoryPlugin />
          <OnChangePlugin onChange={() => {}} />
          <MarkdownShortcutPlugin transformers={Object.values(TRANSFORMERS)} />
          <FloatingToolbar />
        </div>
      </LexicalComposer>

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

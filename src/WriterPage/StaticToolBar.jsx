import React from "react";
import { useLexicalComposerContext } from "@lexical/react/LexicalComposerContext";
import { FORMAT_TEXT_COMMAND } from "lexical";
import "./WriterPage.css";

export default function StaticToolbar() {
  const [editor] = useLexicalComposerContext();

  const applyFormat = (format) => {
    editor.dispatchCommand(FORMAT_TEXT_COMMAND, format);
  };

  return (
    <div
      style={{
        display: "flex",
        gap: "6px",
        padding: "6px",
        backgroundColor: "#fff",
        borderBottom: "1px solid #ccc",
      }}
    >
      <button onClick={() => applyFormat("bold")}>Ж</button>
      <button onClick={() => applyFormat("italic")}>К</button>
    </div>
  );
}

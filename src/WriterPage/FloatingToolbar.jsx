import React, { useEffect, useRef, useState } from "react";
import { $getSelection, $isRangeSelection, FORMAT_TEXT_COMMAND } from "lexical";
import { useLexicalComposerContext } from "@lexical/react/LexicalComposerContext";
import { FORMAT_ELEMENT_COMMAND } from "lexical";
import {
  INSERT_ORDERED_LIST_COMMAND,
  INSERT_UNORDERED_LIST_COMMAND,
} from "@lexical/list";

import "./WriterPage.css";

export function FloatingToolbar() {
  const [editor] = useLexicalComposerContext();
  const [position, setPosition] = useState(null);
  const toolbarRef = useRef(null);

  useEffect(() => {
    return editor.registerUpdateListener(({ editorState }) => {
      editorState.read(() => {
        const selection = $getSelection();
        if ($isRangeSelection(selection) && !selection.isCollapsed()) {
          let rect = null;
          try {
            if (typeof selection.getRangeClientRect === "function") {
              rect = selection.getRangeClientRect();
            }
          } catch (e) {
            console.error("Ошибка в getRangeClientRect:", e);
          }
          if (!rect) {
            try {
              const domSelection = window.getSelection();
              if (domSelection && domSelection.rangeCount > 0) {
                const domRange = domSelection.getRangeAt(0);
                rect = domRange.getBoundingClientRect();
              }
            } catch (e) {
              console.error(
                "Ошибка при вычислении через window.getSelection:",
                e
              );
            }
          }
          if (rect) {
            setPosition({
              top: rect.bottom + window.scrollY + 4, // 4px отступ под выделением
              left: rect.left + window.scrollX + rect.width / 2,
            });
          } else {
            setPosition(null);
          }
        } else {
          setPosition(null);
        }
      });
    });
  }, [editor]);

  const applyFormat = (format) => {
    editor.dispatchCommand(FORMAT_TEXT_COMMAND, format);
  };

  return position ? (
    <div
      ref={toolbarRef}
      style={{
        position: "fixed", // используем fixed, чтобы позиционирование было по viewport
        top: position.top,
        left: position.left,
        transform: "translateX(-50%)",
        backgroundColor: "#fff",
        border: "1px solid #ccc",
        boxShadow: "0 2px 8px rgba(0,0,0,0.2)",
        zIndex: 100,
        display: "flex",
        width: "fit-content",
        height: "fit-content",
      }}
    >
      <div className="bar-but" onClick={() => applyFormat("bold")}>
        Ж
      </div>
      <div className="bar-but" onClick={() => applyFormat("italic")}>
        К
      </div>
      <div
        className="bar-but"
        onClick={() =>
          editor.dispatchCommand(INSERT_UNORDERED_LIST_COMMAND, undefined)
        }
      >
        • Список
      </div>
      <div
        className="bar-but"
        onClick={() =>
          editor.dispatchCommand(INSERT_ORDERED_LIST_COMMAND, undefined)
        }
      >
        1. Список
      </div>
      <div
        className="bar-but"
        onClick={() =>
          editor.dispatchCommand(FORMAT_ELEMENT_COMMAND, "heading1")
        }
      >
        H1
      </div>
      <div
        className="bar-but"
        onClick={() =>
          editor.dispatchCommand(FORMAT_ELEMENT_COMMAND, "heading2")
        }
      >
        H2
      </div>
    </div>
  ) : null;
}

import React from "react";
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
import {
  TRANSFORMERS,
  $convertToMarkdownString,
  ELEMENT_TRANSFORMERS,
  TEXT_FORMAT_TRANSFORMERS,
} from "@lexical/markdown";
import { FloatingToolbar } from "./FloatingToolbar";
import { useLexicalComposerContext } from "@lexical/react/LexicalComposerContext";
import { $getRoot } from "lexical";

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

  const transformers = Array.isArray(ELEMENT_TRANSFORMERS)
    ? [...ELEMENT_TRANSFORMERS, ...TEXT_FORMAT_TRANSFORMERS]
    : Object.values(ELEMENT_TRANSFORMERS).concat(
        Object.values(TEXT_FORMAT_TRANSFORMERS)
      );
  const handleSave = () => {
    editor.getEditorState().read(() => {
      const markdown = $convertToMarkdownString($getRoot(), transformers);
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

export default function WriterPage() {
  return (
    <div style={{ padding: "2rem", maxWidth: "700px", margin: "0 auto" }}>
      <h2 style={{ marginBottom: "1rem" }}>Редактор текста</h2>
      <LexicalComposer initialConfig={editorConfig}>
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
          <SaveButtonMarkdown />
        </div>
      </LexicalComposer>
    </div>
  );
}

using System;
using Dialogue;

public static class EventHandler
{
    public static event Action OnOpenDialoguePanel;

    public static void OpenDialoguePanel()
    {
        OnOpenDialoguePanel?.Invoke();
    }

    public static event Action<DialoguePiece> OnShowDialoguePiece;

    public static void ShowDialoguePiece(DialoguePiece piece)
    {
        OnShowDialoguePiece?.Invoke(piece);
    }
    
    public static event Action OnCloseDialoguePanel;

    public static void CloseDialoguePanel()
    {
        OnCloseDialoguePanel?.Invoke();
    }
}

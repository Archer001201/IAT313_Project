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

    public static event Action<DialogueOption> OnShowDialogueOption;

    public static void ShowDialogueOption(DialogueOption option)
    {
        OnShowDialogueOption?.Invoke(option);
    }

    public static event Action<DialogueOption> OnShowSelectedOption;
    
    public static void ShowSelectedOption(DialogueOption option)
    {
        OnShowSelectedOption?.Invoke(option);
    }

    public static event Action OnNavigationUp;

    public static void NavigationUp()
    {
        OnNavigationUp?.Invoke();
    }

    public static event Action OnNavigationDown;

    public static void NavigationDown()
    {
        OnNavigationDown?.Invoke();
    }
    
    public static event Action OnDestroyOptions;

    public static void DestroyOptions()
    {
        OnDestroyOptions?.Invoke();
    }

    public static event Action<float[]> OnAfterEventEffect;

    public static void AfterEventEffect(float[] effect)
    {
        OnAfterEventEffect?.Invoke(effect);
    }

    public static event Action OnCloseInteractableSign;

    public static void CloseInteractableSign()
    {
        OnCloseInteractableSign?.Invoke();
    }

    public static event Action OnCostActionPoint;

    public static void CostActionPoint()
    {
        OnCostActionPoint?.Invoke();
    }
}

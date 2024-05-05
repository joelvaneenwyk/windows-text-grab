using Text_Grab.Controls;

namespace Text_Grab.UndoRedoOperations;

internal class ChangeWord : Operation, IUndoRedoOperation
{
    public ChangeWord(uint transactionId, WordBorder wordBorder,
        string oldWord, string newWord) : base(transactionId)
    {
        WordBorder = wordBorder;
        OldWord = oldWord;
        NewWord = newWord;

    }

    private readonly WordBorder WordBorder;

    private readonly string OldWord;

    private readonly string NewWord;

    public UndoRedoOperation GetUndoRedoOperation() => UndoRedoOperation.AddWordBorder;

    public void Undo()
    {
        WordBorder.Word = OldWord;
    }

    public void Redo()
    {
        WordBorder.Word = NewWord;
    }
}

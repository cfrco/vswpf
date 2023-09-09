using System.Collections.Generic;
using VsWpf.BoardObject;

namespace VsWpf
{
    enum BoardAction
    {
        Modify, // Attributes and Offset
        Add,
        Remove,
        Swap,
    }

    class BoardHistoryItem
    {
        public int Index { get; set; }
        public BoardAction Action { get; set; }

        public IBoardObject? Original { get; set;}
        public IBoardObject? New { get; set;}

        public int SwapTarget { get; set;}
    }


    class BoardHistory
    {
        private List<BoardHistoryItem> items = new List<BoardHistoryItem>();

        private int current = -1;

        private int maxCount = 100;

        public BoardHistory(int maxCount)
        {
            this.maxCount = maxCount;
        }

        public void Push(BoardHistoryItem item)
        {
            int index = current + 1;
            for (int i = items.Count - 1; i >= index && i >= 0; i--)
            {
                items.RemoveAt(i);
            }

            items.Add(item);
            current = index;

            while (items.Count > maxCount)
            {
                items.RemoveAt(0);
                current--;
            }
        }

        public BoardHistoryItem? Pop()
        {
            if (current < 0 || current >= items.Count)
            {
                return null;
            }

            int index = current;
            current--;
            return items[index];
        }

        public BoardHistoryItem? Forward()
        {
            if (current + 1 >= items.Count)
            {
                return null;
            }

            current++;
            return items[current];
        }
    }
}
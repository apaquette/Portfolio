namespace Models;

class ReverseDateTimeComparer : IComparer<DateTime>
{
    public int Compare(DateTime x, DateTime y) => y.CompareTo(x);
}
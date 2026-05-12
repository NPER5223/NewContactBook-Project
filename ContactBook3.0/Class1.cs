using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewContactBook
{
    // Implements IComparer<Entry> so it can be passed to List.Sort()
    // Sorts by a chosen primary field, breaking ties using the precedence:
    // FirstName > LastName > Phone > Email
    public class ContactComparer : IComparer<Entry>
    {
        // Which field to sort by primarily
        public enum SortField { FirstName, LastName, Phone, Email }

        private readonly SortField _primary;

        public ContactComparer(SortField primary)
        {
            _primary = primary;
        }

        public int Compare(Entry a, Entry b)
        {
            // Helper: null-safe ordinal comparison
            static int Cmp(string x, string y) =>
                string.Compare(x ?? "", y ?? "", StringComparison.OrdinalIgnoreCase);

            int result;

            switch (_primary)
            {
                case SortField.FirstName:
                    // Primary: FirstName
                    result = Cmp(a.FirstName, b.FirstName);
                    if (result != 0) return result;
                    // Tie-break 1: LastName
                    result = Cmp(a.LastName, b.LastName);
                    if (result != 0) return result;
                    // Tie-break 2: Phone
                    result = Cmp(a.PhoneNumber, b.PhoneNumber);
                    if (result != 0) return result;
                    // Tie-break 3: Email
                    return Cmp(a.Email, b.Email);

                case SortField.LastName:
                    // Primary: LastName
                    result = Cmp(a.LastName, b.LastName);
                    if (result != 0) return result;
                    // Tie-break 1: FirstName
                    result = Cmp(a.FirstName, b.FirstName);
                    if (result != 0) return result;
                    // Tie-break 2: Phone
                    result = Cmp(a.PhoneNumber, b.PhoneNumber);
                    if (result != 0) return result;
                    // Tie-break 3: Email
                    return Cmp(a.Email, b.Email);

                case SortField.Phone:
                    // Primary: Phone
                    result = Cmp(a.PhoneNumber, b.PhoneNumber);
                    if (result != 0) return result;
                    // Tie-break 1: FirstName
                    result = Cmp(a.FirstName, b.FirstName);
                    if (result != 0) return result;
                    // Tie-break 2: LastName
                    result = Cmp(a.LastName, b.LastName);
                    if (result != 0) return result;
                    // Tie-break 3: Email
                    return Cmp(a.Email, b.Email);

                case SortField.Email:
                    // Primary: Email
                    result = Cmp(a.Email, b.Email);
                    if (result != 0) return result;
                    // Tie-break 1: FirstName
                    result = Cmp(a.FirstName, b.FirstName);
                    if (result != 0) return result;
                    // Tie-break 2: LastName
                    result = Cmp(a.LastName, b.LastName);
                    if (result != 0) return result;
                    // Tie-break 3: Phone
                    return Cmp(a.PhoneNumber, b.PhoneNumber);

                default:
                    return 0;
            }
        }
    }
}


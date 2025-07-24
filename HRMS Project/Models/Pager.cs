using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMSProject.Models
{
    public class Pager
    {
        public int StartPage { get; }
        public int EndPage { get; }
        public int CurrentPage { get; set; }

        public Pager(int currentPage, int totalItems, int pageSize, int maxWidth = 5)
        {
            CurrentPage = currentPage;
            int totalPages = Convert.ToInt32(Math.Ceiling((float)totalItems/pageSize));
            int itemBeforenAfter = maxWidth / 2;
            StartPage = Math.Max(CurrentPage - itemBeforenAfter, 1);
            if (maxWidth % 2 != 0) EndPage++; // Put this condition before 'StartPage' variable and increment it if want to make the currentPage closer to endpage when width is even.
            EndPage = Math.Min(totalPages, CurrentPage + itemBeforenAfter);
        }
    }
}
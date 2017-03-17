using FakeBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FakeBlog.DAL
{
    public class IRepository
    {
        void AddDraft(string title, string contents, ApplicationUser author);
    }
}
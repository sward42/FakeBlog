using FakeBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeBlog.Controllers.Contracts
{
    interface IDraftRepository
    {
        List<Draft> ReturnDrafts(string authorId);
        Draft ReturnSingleDraft(int draftId);

        void CreateDraft(string title, string contents, ApplicationUser author);
        void PublishDraft(int draftId);

        bool DeleteDraft(int draftId);

        bool EditDraft(int draftId, string newContents);
    }
}

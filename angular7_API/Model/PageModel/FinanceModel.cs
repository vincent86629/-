using angular_API.Model.EFModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace angular_API.Model.PageModel
{

    public class FinanceModel
    {
        public class FinanceListData
        {
            public List<Row> Rows { get; set; } = new List<Row>();
            public class Row
            {
                public int Id { get; set; }
                public string YearMonth { get; set; }
                public string Community { get; set; }
                public string CreateBy { get; set; }
                public DateTime UpdateTime { get; set; }
                public string Status { get; set; }
            }
        }
        public class FinanceListDataQuery
        {
            public int? CommunityId { get; set; }
            public string YearMonth { get; set; }
            public int? StatusId { get; set; }
            public string CreateBy { get; set; }
        }
        public class FinanceEditData
        {
            public int Id { get; set; }
            public string Permission { get; set; }
            public string YearMonth { get; set; }
            public int LastMonthBalance { get; set; }
            public int ThisMonthBalance { get; set; }
            public int StatusId { get; set; }
            public int CreateBy { get; set; }
            public List<Row> BankSaving { get; set; }
            public List<Block> Blocks { get; set; }
            public List<FinanceFile> Files { get; set; }

            public FinanceEditData()
            {
                BankSaving = new List<Row>();
                Blocks = new List<Block>();
                Files = new List<FinanceFile>();
            }

        }
        public class Block
        {
            public string BlockName { get; set; }
            public string TotalName { get; set; }
            public int Total { get; set; }
            public List<Row> Rows { get; set; } = new List<Row>();
        }
        public class FinanceFile
        {
            public int Id { get; set; }
            public string Path { get; set; }
        }
        public class Row
        {
            public string Date { get; set; }
            public string Name { get; set; }
            public int? Value { get; set; } = null;
        }
    }

}

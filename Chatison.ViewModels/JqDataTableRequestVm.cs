using System.Collections.Generic;
using System.Linq;

namespace Chatison.ViewModels
{
    public class JqDataTableRequestVm
    {
        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public JqDataTableSearchVm Search { get; set; }

        public List<JqDataTableSortVm> Order { get; set; }

        public List<JqDataTableColumnVm> Columns { get; set; }

        public string GetSortExpression()
        {
            var columnIndex = Order.FirstOrDefault()?.Column ?? 0;
            var sortDir = Order.FirstOrDefault()?.Dir ?? "asc";
            var columnName = Columns[columnIndex].Data;
            return $"{columnName} {sortDir}";
        }
    }
}

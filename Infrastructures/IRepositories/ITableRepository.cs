using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.IRepositories;

public interface ITableRepository
{
    Task<IEnumerable<Table>> GetTablesAsync();
    Task<Table?> GetTableByIdAsync(Guid id);
    void AddTable(Table table);
    void RemoveTable(Table table);
}

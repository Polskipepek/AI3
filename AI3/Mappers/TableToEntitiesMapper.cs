using AI3.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI3.Mappers {
    public static class TableToEntitiesMapper {
        public static IEnumerable<Entity> Map(DataTable table) {
            List<Entity> entities = new();
            foreach (DataRow row in table.Rows) {
                var entity = new Entity();
                foreach (DataColumn column in table.Columns) {
                    if (column.ColumnName.Equals("decision")) continue;
                    entity.Attributes.Add(new ILAAttribute() { Name = column.ColumnName, Value = row[column.ColumnName] });
                }
                entity.DecisionAttribute = row[table.Columns.Count-1] + "";
                entities.Add(entity);
            }
            return entities;
        }
    }
}

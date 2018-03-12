using System.Collections.Generic;
using System.Linq;
using EFMultiTenant.Models;

namespace EFMultiTenantTest
{
    public class DatabaseDeleter
        // Database cleanup
        // Based on https://lostechies.com/jimmybogard/2012/10/18/isolating-database-data-in-integration-tests/
    {
        private static readonly string[] IgnoredTables = { "dbo.__MigrationHistory" };
        private static string[] _tablesToDelete;
        private static string _deleteSql;
        private static readonly object LockObj = new object();
        private static bool _initialized;

        public void DeleteAllData()
        {
            using (var context = new EFMultiTenantDbContext())
            {
                if (_deleteSql == null)
                {
                    BuildDeleteTables(context);
                }
                //                System.Console.WriteLine(_deleteSql);
                context.Database.ExecuteSqlCommand(_deleteSql);
            }
        }

        public static string[] GetTables()
        {
            return _tablesToDelete;
        }

        private void BuildDeleteTables(EFMultiTenantDbContext dbContext)
        {
            if (!_initialized)
                lock (LockObj)
                {
                    if (!_initialized)
                    {
                        var allTables = GetAllTables(dbContext);

                        var allRelationships = GetRelationships(dbContext);

                        _tablesToDelete = BuildTableList(allTables, allRelationships);

                        _deleteSql = BuildTableSql(_tablesToDelete);

                        _initialized = true;
                    }
                }
        }

        private static string BuildTableSql(IEnumerable<string> tablesToDelete)
        {
            var completeQuery = "";
            foreach (var tableName in tablesToDelete)
                completeQuery += string.Format("delete from {0};", tableName);
            return completeQuery;
        }

        private static string[] BuildTableList(ICollection<string> allTables,
            ICollection<Relationship> allRelationships)
        {
            var tablesToDelete = new List<string>();

            while (allTables.Any())
            {
                var leafTables = allTables.Except(allRelationships.Where(rel1 =>
                        rel1.PrimaryKeyTable != rel1.ForeignKeyTable).Select(rel2 =>
                        rel2.PrimaryKeyTable))
                    .ToArray();

                tablesToDelete.AddRange(leafTables);
                foreach (var leafTable in leafTables)
                {
                    allTables.Remove(leafTable);
                    var relToRemove = allRelationships.Where(rel =>
                            rel.ForeignKeyTable == leafTable)
                        .ToArray();
                    foreach (var rel in relToRemove)
                        allRelationships.Remove(rel);
                }
            }

            return tablesToDelete.ToArray();
        }

        private static List<Relationship> GetRelationships(EFMultiTenantDbContext dbContext)
        {
            var otherquery = dbContext.Database.SqlQuery<Relationship>(
                @"select
	ss_pk.name + '.' + so_pk.name as PrimaryKeyTable,
    ss_fk.name + '.' + so_fk.name as ForeignKeyTable
from
	sysforeignkeys sfk
		inner join sys.objects so_pk on sfk.rkeyid = so_pk.object_id
		inner join sys.schemas ss_pk on so_pk.schema_id = ss_pk.schema_id
		inner join sys.objects so_fk on sfk.fkeyid = so_fk.object_id
		inner join sys.schemas ss_fk on so_fk.schema_id = ss_fk.schema_id
order by
	so_pk.name,
    so_fk.name");

            return otherquery.ToList();
        }

        private static List<string> GetAllTables(EFMultiTenantDbContext dbContext)
        {
            var query =
                dbContext.Database.SqlQuery<string>(
                    "select schemas.name + '.' + tables.name from sys.tables join sys.schemas on (schemas.schema_id = tables.schema_id)");

            return query.Except(IgnoredTables).ToList();
        }

        private class Relationship
        {
            public string PrimaryKeyTable { get; private set; }
            public string ForeignKeyTable { get; private set; }
        }
    }
}
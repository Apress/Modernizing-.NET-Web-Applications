using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ModernizationDemo.EfCoreTests.Model;

public class KeyNamingConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var entity in modelBuilder.Metadata.GetEntityTypes())
        {
            foreach (var index in entity.GetDeclaredIndexes())
            {
                index.SetDatabaseName($"IX_{string.Join("_", index.Properties.Select(p => p.GetColumnName()))}");
            }
            foreach (var key in entity.GetDeclaredKeys().Where(k => k.IsPrimaryKey())) 
            {
                if (key.IsPrimaryKey())
                {
                    key.SetName($"PK_dbo.{entity.GetTableName()}");
                }
            }
            foreach (var key in entity.GetDeclaredReferencingForeignKeys())
            {
                key.SetConstraintName($"FK_dbo.{key.DeclaringEntityType.GetTableName()}_dbo.{key.PrincipalEntityType.GetTableName()}_{string.Join("_", key.Properties.Select(p => p.GetColumnName()))}");
            }
        }
    }
}